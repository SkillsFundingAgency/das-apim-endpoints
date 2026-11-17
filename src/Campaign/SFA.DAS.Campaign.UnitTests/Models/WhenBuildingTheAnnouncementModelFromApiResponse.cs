using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Models
{
    public class WhenBuildingTheAnnouncementModelFromApiResponse
    {
        [Test]
        public void Then_If_No_Items_Returned_Then_Null_Returned()
        {
            var source = new CmsContent { Items = new List<Item>(), Total = 1 };

            var actual = new AnnouncementModel().Build(source);

            actual.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_If_Total_Is_Zero_Then_Null_Returned(CmsContent source)
        {
            source.Total = 0;

            var actual = new AnnouncementModel().Build(source);

            actual.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_If_The_Announcement_Has_No_Content_Then_It_Is_Built_With_No_Items(CmsContent source)
        {
            source.Items.First().Fields.Content = null;

            var actual = new AnnouncementModel().Build(source);

            actual.MainContent.Should().NotBeNull();
            actual.MainContent.Headline.Should().Be(source.Items.First().Fields.Headline);
            actual.MainContent.Items.Should().BeEmpty();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Announcement_Is_Built(CmsContent source, string contentValue)
        {
            var item = source.Items.First();
            var category = source.Includes.Entry.First();
            item.Fields.Category = new LandingPage { Sys = new LandingPageSys { Id = category.Sys.Id } };
            var tab = source.Includes.Entry.Last();
            tab.Sys.ContentType.Sys.Id = "tab";
            item.Fields.Sections = new List<TabbedContent> { new TabbedContent { Sys = new LandingPageSys { Id = tab.Sys.Id } } };
            foreach (var subContentItems in item.Fields.Content.Content)
            {
                subContentItems.NodeType = "paragraph";
                subContentItems.Content = new List<ContentDefinition>
                {
                    new ContentDefinition { NodeType = "text", Value = contentValue }
                };
            }

            var actual = new AnnouncementModel().Build(source);

            actual.MainContent.Id.Should().Be(item.Sys.Id);
            actual.MainContent.Headline.Should().Be(item.Fields.Headline);
            actual.MainContent.Slug.Should().Be(item.Fields.Slug);
            actual.MainContent.Intro.Should().Be(item.Fields.Intro);
            actual.MainContent.PublishedDate.Should().Be(item.Fields.Date);
            actual.MainContent.UpdatedDate.Should().Be(item.Fields.UpdatedDate);
            actual.MainContent.Audience.Should().Be(item.Fields.Audience);
            actual.MainContent.Category.Should().Be(category.Fields.Title);
            actual.Sections.Should().ContainSingle();
            actual.Sections.Single().Id.Should().Be(tab.Sys.Id);
            actual.Sections.Single().TabName.Should().Be(tab.Fields.TabName);
            actual.MainContent.Items.Should().HaveCount(item.Fields.Content.Content.Count);
            actual.MainContent.Items.Should().OnlyContain(c => c.Type == "paragraph" && c.Values.Contains(contentValue));
        }
    }
}
