using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Models
{
    public class WhenBuildingTheHomepageModelFromApiResponse
    {
        [Test]
        public void Then_If_No_Items_Returned_Then_Null_Returned()
        {
            var source = new CmsContent { Items = new List<Item>(), Total = 1 };

            var actual = new HomepageModel().Build(source);

            actual.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_If_Total_Is_Zero_Then_Null_Returned(CmsContent source)
        {
            source.Total = 0;

            var actual = new HomepageModel().Build(source);

            actual.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Page_Fields_Are_Mapped(CmsContent source)
        {
            var item = source.Items.First();

            var actual = new HomepageModel().Build(source);

            actual.PageTitle.Should().Be(item.Fields.PageTitle);
            actual.BrowserTitle.Should().Be(item.Fields.BrowserTitle);
            actual.IntroText.Should().Be(item.Fields.IntroText);
        }

        [Test, RecursiveMoqAutoData]
        public void Then_If_There_Are_No_Feature_Card_Links_Then_The_List_Is_Empty(CmsContent source)
        {
            source.Items.First().Fields.FeatureCards = null;

            var actual = new HomepageModel().Build(source);

            actual.FeatureCards.Should().BeEmpty();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_Feature_Cards_Are_Resolved_From_Includes_In_Link_Order(CmsContent source)
        {
            var item = source.Items.First();
            var entries = source.Includes.Entry;
            // Link the cards in reverse include order so ordering is observable, and drop one so unmatched links are skipped.
            item.Fields.FeatureCards = entries
                .AsEnumerable()
                .Reverse()
                .Select(e => new CardItem { Sys = new LandingPageSys { Id = e.Sys.Id } })
                .Append(new CardItem { Sys = new LandingPageSys { Id = "not-in-includes" } })
                .ToList();
            // First card links to an internal article page (the last included entry), which in turn has a parent landing page.
            var page = entries.Last();
            page.Sys.ContentType.Sys.Id = "article";
            var parent = entries[entries.Count - 2];
            page.Fields.LandingPage = new LandingPage { Sys = new LandingPageSys { Id = parent.Sys.Id } };
            entries.First().Fields.InternalPage = new LandingPage { Sys = new LandingPageSys { Id = page.Sys.Id } };

            var actual = new HomepageModel().Build(source);

            actual.FeatureCards.Should().HaveCount(entries.Count);
            actual.FeatureCards.Select(c => c.Id).Should().Equal(entries.AsEnumerable().Reverse().Select(e => e.Sys.Id));
            var first = actual.FeatureCards.Last();
            first.Heading.Should().Be(entries.First().Fields.Heading);
            first.Description.Should().Be(entries.First().Fields.Description);
            first.Audience.Should().Be(entries.First().Fields.Audience);
            first.LinkType.Should().Be(entries.First().Fields.LinkType);
            first.ExternalUrl.Should().Be(entries.First().Fields.ExternalUrl);
            first.InternalPage.Should().BeEquivalentTo(new UrlDetails
            {
                Title = page.Fields.Title,
                Slug = page.Fields.Slug,
                Hub = page.Fields.HubType,
                PageType = "Article",
                ParentSlug = parent.Fields.Slug
            });
        }
    }
}
