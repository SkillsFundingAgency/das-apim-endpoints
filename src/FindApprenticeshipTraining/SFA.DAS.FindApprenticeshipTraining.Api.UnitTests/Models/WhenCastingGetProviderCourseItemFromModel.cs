using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
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

            shortlistItem.ProviderDetails.EmployerFeedback.ReviewCount = 129;
            shortlistItem.ProviderDetails.EmployerFeedback.Stars = 3;
            shortlistItem.ProviderDetails.ApprenticeFeedback.ReviewCount = 129;
            shortlistItem.ProviderDetails.ApprenticeFeedback.Stars = 3;

            var actual = new GetProviderCourseItem().Map(shortlistItem);

            actual.Should().BeEquivalentTo(shortlistItem.Course, options => options.ExcludingMissingMembers());
            actual.Website.Should().Be(shortlistItem.ProviderDetails.StandardInfoUrl);
            actual.ProviderId.Should().Be(shortlistItem.ProviderDetails.Ukprn);
            actual.ProviderAddress.Should().BeEquivalentTo(shortlistItem.ProviderDetails.ProviderAddress);
            actual.EmployerFeedback.TotalEmployerResponses.Should().Be(129);
            actual.EmployerFeedback.TotalFeedbackRating.Should().Be(3);
            actual.ApprenticeFeedback.TotalApprenticeResponses.Should().Be(129);
            actual.ApprenticeFeedback.TotalFeedbackRating.Should().Be(3);
        }

        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Matching_AchievementRates_With_Level_Higher_Than_Three(
            InnerApi.Responses.GetShortlistItem shortlistItem,
            GetAchievementRateItem item,
            GetAchievementRateItem item2)
        {
            shortlistItem.Course.Level = 5;
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
        public void Then_Maps_Fields_Appropriately_Matching_AchievementRates_And_Level(
            InnerApi.Responses.GetShortlistItem shortlistItem,
            GetAchievementRateItem item,
            GetAchievementRateItem item2)
        {
            shortlistItem.Course.Level = 2;
            item.Level = "Two";
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
        public void Then_Maps_Fields_Appropriately_Returning_Null_For_AchievementRate_Data_If_No_AchievementRates_Data(InnerApi.Responses.GetShortlistItem shortlistItem)
        {
            shortlistItem.ProviderDetails.AchievementRates = new List<GetAchievementRateItem>();

            var response = new GetProviderCourseItem().Map(shortlistItem);

            using (new AssertionScope())
            {
                response.OverallCohort.Should().BeNull();
                response.OverallAchievementRate.Should().BeNull();
            }
        }
    }
}