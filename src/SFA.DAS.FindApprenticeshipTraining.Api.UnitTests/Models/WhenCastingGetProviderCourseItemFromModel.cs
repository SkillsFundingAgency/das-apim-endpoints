using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetProviderCourseItemFromModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(InnerApi.Responses.GetShortlistItem shortlistItem)
        {
            shortlistItem.Provider.FeedbackRatings = new List<GetFeedbackRatingItem>
            {
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Good",
                    FeedbackCount = 92,
                },
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Excellent",
                    FeedbackCount = 29,
                },
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Poor",
                    FeedbackCount = 7,
                },
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Very Poor",
                    FeedbackCount = 1,
                }
            };

            var actual =new GetProviderCourseItem().Map(shortlistItem);
            
            actual.Should().BeEquivalentTo(shortlistItem.Course, options => options.ExcludingMissingMembers());
            actual.Website.Should().Be(shortlistItem.Provider.ContactUrl);
            actual.ProviderId.Should().Be(shortlistItem.Provider.Ukprn);
            actual.ProviderAddress.Should().BeEquivalentTo(shortlistItem.Provider.ProviderAddress);
            actual.Feedback.TotalEmployerResponses.Should().Be(129);
            actual.Feedback.TotalFeedbackRating.Should().Be(3);
        }

        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Matching_AchievementRates_With_Sector_And_Level_Higher_Than_Three(
            InnerApi.Responses.GetShortlistItem shortlistItem,
            GetAchievementRateItem item, 
            GetAchievementRateItem item2)
        {
            shortlistItem.Course.Level = 5;
            item.SectorSubjectArea = shortlistItem.Course.Route;
            item.Level = "AllLevels";
            shortlistItem.Provider.AchievementRates = new List<GetAchievementRateItem>
            {
                item,
                item2
            };
            
            var response = new GetProviderCourseItem().Map(shortlistItem);

            response.Name.Should().Be(shortlistItem.Provider.Name);
            response.TradingName.Should().Be(shortlistItem.Provider.TradingName);
            response.ProviderId.Should().Be(shortlistItem.Provider.Ukprn);
            response.OverallCohort.Should().Be(item.OverallCohort);
            response.OverallAchievementRate.Should().Be(item.OverallAchievementRate);
        }

        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Matching_AchievementRates_With_Sector_And_Level(
            InnerApi.Responses.GetShortlistItem shortlistItem,
            GetAchievementRateItem item, 
            GetAchievementRateItem item2,
            GetAchievementRateItem item3)
        {
            shortlistItem.Course.Level = 2;
            item.SectorSubjectArea = shortlistItem.Course.Route;
            item.Level = "Two";
            shortlistItem.Provider.AchievementRates = new List<GetAchievementRateItem>
            {
                item,
                item2
            };
            item3.SectorSubjectArea = shortlistItem.Course.Route;
            item3.Level = "Two";

            var response = new GetProviderCourseItem().Map(shortlistItem);

            response.Name.Should().Be(shortlistItem.Provider.Name);
            response.TradingName.Should().Be(shortlistItem.Provider.TradingName);
            response.ProviderId.Should().Be(shortlistItem.Provider.Ukprn);
            response.OverallCohort.Should().Be(item.OverallCohort);
            response.OverallAchievementRate.Should().Be(item.OverallAchievementRate);
        }
        
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Returning_Null_For_AchievementRate_Data_If_No_Matching_No_AchievementRates(
            InnerApi.Responses.GetShortlistItem shortlistItem,
            GetAchievementRateItem item, 
            GetAchievementRateItem item2)
        {
            shortlistItem.Provider.AchievementRates = new List<GetAchievementRateItem>
            {
                item,
                item2
            };

            var response = new GetProviderCourseItem().Map(shortlistItem);

            response.Name.Should().Be(shortlistItem.Provider.Name);
            response.TradingName.Should().Be(shortlistItem.Provider.TradingName);
            response.ProviderId.Should().Be(shortlistItem.Provider.Ukprn);
            response.OverallCohort.Should().BeNull();
            response.OverallAchievementRate.Should().BeNull();
        }
    }
}