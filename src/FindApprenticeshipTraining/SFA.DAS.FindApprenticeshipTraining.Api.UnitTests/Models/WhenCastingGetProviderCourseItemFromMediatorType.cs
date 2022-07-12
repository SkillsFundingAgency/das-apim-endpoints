using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetProviderCourseItemFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetTrainingCourseProviderResult providerStandardItem)
        {
            providerStandardItem.ProviderStandard.EmployerFeedback.FeedbackRatings = new List<GetEmployerFeedbackRatingItem>
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

            providerStandardItem.ProviderStandard.ApprenticeFeedback.ProviderRating = new List<GetApprenticeFeedbackRatingItem>
            {
                new GetApprenticeFeedbackRatingItem
                {
                    Rating = "Good",
                    Count = 92,
                },
                new GetApprenticeFeedbackRatingItem
                {
                    Rating = "Excellent",
                    Count = 29,
                },
                new GetApprenticeFeedbackRatingItem
                {
                    Rating = "Poor",
                    Count = 7,
                },
                new GetApprenticeFeedbackRatingItem
                {
                    Rating = "Very Poor",
                    Count = 1,
                }
            };

            var actual =new GetProviderCourseItem().Map(providerStandardItem, "",1, true);
            
            actual.Should().BeEquivalentTo(providerStandardItem.Course, options => options.ExcludingMissingMembers());

            actual.Website.Should().Be(providerStandardItem.ProviderStandard.StandardInfoUrl);
            actual.ProviderId.Should().Be(providerStandardItem.ProviderStandard.Ukprn);
            actual.ProviderAddress.Should().BeEquivalentTo(providerStandardItem.ProviderStandard.ProviderAddress);
            actual.EmployerFeedback.TotalEmployerResponses.Should().Be(129);
            actual.EmployerFeedback.TotalFeedbackRating.Should().Be(3);
            actual.ApprenticeFeedback.TotalApprenticeResponses.Should().Be(129);
            actual.ApprenticeFeedback.TotalFeedbackRating.Should().Be(3);
            actual.MarketingInfo.Should().Be(providerStandardItem.ProviderStandard.MarketingInfo);
        }
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Matching_AchievementRates_With_Sector_And_Level_Higher_Than_Three(string sectorSubjectArea,
            GetTrainingCourseProviderResult source, GetAchievementRateItem item, GetAchievementRateItem item2)
        {
            item.SectorSubjectArea = sectorSubjectArea;
            item.Level = "AllLevels";
            source.ProviderStandard.AchievementRates = new List<GetAchievementRateItem>
            {
                item,
                item2
            };
            
            var response = new GetProviderCourseItem().Map(source, sectorSubjectArea,5, true);

            response.Name.Should().Be(source.ProviderStandard.Name);
            response.TradingName.Should().Be(source.ProviderStandard.TradingName);
            response.ProviderId.Should().Be(source.ProviderStandard.Ukprn);
            response.OverallCohort.Should().Be(item.OverallCohort);
            response.OverallAchievementRate.Should().Be(item.OverallAchievementRate);

        }
        [Test, AutoData]

        public void Then_Maps_Fields_Appropriately_Matching_AchievementRates_With_Sector_And_Level(string sectorSubjectArea,
            GetTrainingCourseProviderResult source, GetAchievementRateItem item, GetAchievementRateItem item2,GetAchievementRateItem item3)
        {
            item.SectorSubjectArea = sectorSubjectArea;
            item.Level = "Two";
            source.ProviderStandard.AchievementRates = new List<GetAchievementRateItem>
            {
                item,
                item2
            };
            item3.SectorSubjectArea = sectorSubjectArea;
            item3.Level = "Two";
            source.OverallAchievementRates = new List<GetAchievementRateItem>
            {
                item2,
                item3
            };
            
            var response = new GetProviderCourseItem().Map(source, sectorSubjectArea,2, true);

            response.Name.Should().Be(source.ProviderStandard.Name);
            response.TradingName.Should().Be(source.ProviderStandard.TradingName);
            response.ProviderId.Should().Be(source.ProviderStandard.Ukprn);
            response.OverallCohort.Should().Be(item.OverallCohort);
            response.NationalOverallCohort.Should().Be(item3.OverallCohort);
            response.OverallAchievementRate.Should().Be(item.OverallAchievementRate);
            response.NationalOverallAchievementRate.Should().Be(item3.OverallAchievementRate);

        }
        
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Returning_Null_For_AchievementRate_Data_If_No_Matching_No_AchievementRates(string sectorSubjectArea,
            GetTrainingCourseProviderResult source, GetAchievementRateItem item, GetAchievementRateItem item2)
        {
            source.ProviderStandard.AchievementRates = new List<GetAchievementRateItem>
            {
                item,
                item2
            };
            source.OverallAchievementRates = new List<GetAchievementRateItem>
            {
                item,
                item2
            };
            
            var response = new GetProviderCourseItem().Map(source, sectorSubjectArea, 1, true);

            response.Name.Should().Be(source.ProviderStandard.Name);
            response.TradingName.Should().Be(source.ProviderStandard.TradingName);
            response.ProviderId.Should().Be(source.ProviderStandard.Ukprn);
            response.OverallCohort.Should().BeNull();
            response.NationalOverallCohort.Should().BeNull();
            response.OverallAchievementRate.Should().BeNull();
            response.NationalOverallAchievementRate.Should().BeNull();

        }
    }
}