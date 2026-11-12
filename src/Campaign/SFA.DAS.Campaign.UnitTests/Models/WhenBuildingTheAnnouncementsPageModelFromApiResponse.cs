using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Models
{
    public class WhenBuildingTheAnnouncementsPageModelFromApiResponse
    {
        [Test]
        public void Then_If_No_Items_Returned_Then_Null_Returned()
        {
            var source = new CmsContent { Items = new List<Item>(), Total = 1 };

            var actual = new AnnouncementsPageModel().Build(source);

            actual.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_If_Total_Is_Zero_Then_Null_Returned(CmsContent source)
        {
            source.Total = 0;

            var actual = new AnnouncementsPageModel().Build(source);

            actual.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Summary_Fields_Are_Mapped(CmsContent source)
        {
            // Point every item's category link at the first included entry so the title can be resolved.
            var category = source.Includes.Entry.First();
            foreach (var item in source.Items)
            {
                item.Fields.Category = new LandingPage { Sys = new LandingPageSys { Id = category.Sys.Id } };
            }

            var actual = new AnnouncementsPageModel().Build(source);

            actual.Announcements.Should().HaveCount(source.Items.Count);
            foreach (var item in source.Items)
            {
                actual.Announcements.Should().ContainEquivalentOf(new AnnouncementsPageModel.AnnouncementSummary
                {
                    Id = item.Sys.Id,
                    Headline = item.Fields.Headline,
                    Slug = item.Fields.Slug,
                    Intro = item.Fields.Intro,
                    PublishedDate = item.Fields.Date,
                    Audience = item.Fields.Audience,
                    Category = category.Fields.Title
                });
            }
        }

        [Test, RecursiveMoqAutoData]
        public void Then_If_The_Category_Is_Not_In_The_Includes_Then_Category_Is_Null(CmsContent source)
        {
            var actual = new AnnouncementsPageModel().Build(source);

            actual.Announcements.Should().OnlyContain(a => a.Category == null);
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Announcements_Are_Ordered_By_Published_Date_Descending(CmsContent source)
        {
            var baseDate = new DateTime(2026, 1, 1);
            for (var i = 0; i < source.Items.Count; i++)
            {
                source.Items[i].Fields.Date = baseDate.AddDays(i);
            }

            var actual = new AnnouncementsPageModel().Build(source);

            actual.Announcements.Select(a => a.PublishedDate).Should().BeInDescendingOrder();
            actual.Announcements.First().Slug.Should().Be(source.Items.Last().Fields.Slug);
        }
    }
}
