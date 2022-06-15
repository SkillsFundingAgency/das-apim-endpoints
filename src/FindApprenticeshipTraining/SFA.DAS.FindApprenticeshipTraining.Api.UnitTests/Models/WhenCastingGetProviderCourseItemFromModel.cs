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
            shortlistItem.ProviderDetails.EmployerFeedback.FeedbackRatings = new List<GetEmployerFeedbackRatingItem>
            {
                new GetEmployerFeedbackRatingItem
                {
                    FeedbackName = "Good",
                    FeedbackCount = 92,
                },
                new GetEmployerFeedbackRatingItem
                {
                    FeedbackName = "Excellent",
                    FeedbackCount = 29,
                },
                new GetEmployerFeedbackRatingItem
                {
                    FeedbackName = "Poor",
                    FeedbackCount = 7,
                },
                new GetEmployerFeedbackRatingItem
                {
                    FeedbackName = "Very Poor",
                    FeedbackCount = 1,
                }
            };

            var actual =new GetProviderCourseItem().Map(shortlistItem);
            
            actual.Should().BeEquivalentTo(shortlistItem.Course, options => options.ExcludingMissingMembers());
            actual.Website.Should().Be(shortlistItem.ProviderDetails.StandardInfoUrl);
            actual.ProviderId.Should().Be(shortlistItem.ProviderDetails.Ukprn);
            actual.ProviderAddress.Should().BeEquivalentTo(shortlistItem.ProviderDetails.ProviderAddress);
            actual.EmployerFeedback.TotalEmployerResponses.Should().Be(129);
            actual.EmployerFeedback.TotalFeedbackRating.Should().Be(3);
        }

        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Matching_AchievementRates_With_Sector_And_Level_Higher_Than_Three(
            InnerApi.Responses.GetShortlistItem shortlistItem,
            GetAchievementRateItem item, 
            GetAchievementRateItem item2)
        {
            shortlistItem.Course.Level = 5;
            item.SectorSubjectArea = shortlistItem.Course.SectorSubjectAreaTier2Description;
            item.Level = "AllLevels";
            shortlistItem.ProviderDetails.AchievementRates = new List<GetAchievementRateItem>
            {
                item,
                item2
            };
            
            var response = new GetProviderCourseItem().Map(shortlistItem);

            response.Name.Should().Be(shortlistItem.ProviderDetails.Name);
            response.TradingName.Should().Be(shortlistItem.ProviderDetails.TradingName);
            response.ProviderId.Should().Be(shortlistItem.ProviderDetails.Ukprn);
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
            item.SectorSubjectArea = shortlistItem.Course.SectorSubjectAreaTier2Description;
            item.Level = "Two";
            shortlistItem.ProviderDetails.AchievementRates = new List<GetAchievementRateItem>
            {
                item,
                item2
            };
            item3.SectorSubjectArea = shortlistItem.Course.SectorSubjectAreaTier2Description;
            item3.Level = "Two";

            var response = new GetProviderCourseItem().Map(shortlistItem);

            response.Name.Should().Be(shortlistItem.ProviderDetails.Name);
            response.TradingName.Should().Be(shortlistItem.ProviderDetails.TradingName);
            response.ProviderId.Should().Be(shortlistItem.ProviderDetails.Ukprn);
            response.OverallCohort.Should().Be(item.OverallCohort);
            response.OverallAchievementRate.Should().Be(item.OverallAchievementRate);
        }
        
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Returning_Null_For_AchievementRate_Data_If_No_Matching_No_AchievementRates(
            InnerApi.Responses.GetShortlistItem shortlistItem,
            GetAchievementRateItem item, 
            GetAchievementRateItem item2)
        {
            shortlistItem.ProviderDetails.AchievementRates = new List<GetAchievementRateItem>
            {
                item,
                item2
            };

            var response = new GetProviderCourseItem().Map(shortlistItem);

            response.Name.Should().Be(shortlistItem.ProviderDetails.Name);
            response.TradingName.Should().Be(shortlistItem.ProviderDetails.TradingName);
            response.ProviderId.Should().Be(shortlistItem.ProviderDetails.Ukprn);
            response.OverallCohort.Should().BeNull();
            response.OverallAchievementRate.Should().BeNull();
        }
    }
}