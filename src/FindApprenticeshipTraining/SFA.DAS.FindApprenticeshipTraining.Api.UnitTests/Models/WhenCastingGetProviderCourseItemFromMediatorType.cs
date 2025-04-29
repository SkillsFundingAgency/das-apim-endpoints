using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
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
            providerStandardItem.ProviderStandard.EmployerFeedback.ReviewCount = 129;
            providerStandardItem.ProviderStandard.EmployerFeedback.Stars = 3;
            providerStandardItem.ProviderStandard.ApprenticeFeedback.ReviewCount = 129;
            providerStandardItem.ProviderStandard.ApprenticeFeedback.Stars = 3;

            var actual = new GetProviderCourseItem().Map(providerStandardItem, 1, true);

            actual.Should().BeEquivalentTo(providerStandardItem.Course, options => options.ExcludingMissingMembers());

            actual.Website.Should().Be(providerStandardItem.ProviderStandard.StandardInfoUrl);
            actual.ProviderId.Should().Be(providerStandardItem.ProviderStandard.Ukprn);
            actual.ProviderAddress.Should().BeEquivalentTo(providerStandardItem.ProviderStandard.ProviderAddress);
            actual.EmployerFeedback.TotalEmployerResponses.Should().Be(129);
            actual.EmployerFeedback.TotalFeedbackRating.Should().Be(3);
            actual.ApprenticeFeedback.TotalApprenticeResponses.Should().Be(129);
            actual.ApprenticeFeedback.TotalFeedbackRating.Should().Be(3);
            actual.MarketingInfo.Should().Be(providerStandardItem.ProviderStandard.MarketingInfo);
            actual.DeliveryModes.Count.Should().Be(providerStandardItem.ProviderStandard.DeliveryModels.ToList().Count);
            Assert.That(actual.DeliveryModes.Any(x => x.Address1.Contains(providerStandardItem.ProviderStandard.DeliveryModels.ToList().First().Address1)), Is.True);
        }

        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Matching_AchievementRates_With_Sector_And_Level_Higher_Than_Three(GetTrainingCourseProviderResult source, GetAchievementRateItem item, GetAchievementRateItem item2)
        {
            item.Level = "AllLevels";
            source.ProviderStandard.AchievementRates = new List<GetAchievementRateItem>
            {
                item,
                item2
            };

            var response = new GetProviderCourseItem().Map(source, 5, true);

            response.Name.Should().Be(source.ProviderStandard.Name);
            response.TradingName.Should().Be(source.ProviderStandard.TradingName);
            response.ProviderId.Should().Be(source.ProviderStandard.Ukprn);
            response.OverallCohort.Should().Be(item.OverallCohort);
            response.OverallAchievementRate.Should().Be(item.OverallAchievementRate);

        }

        [Test, AutoData]
        public void Then_Maps_AchievementRates_From_First_Item(GetTrainingCourseProviderResult source, GetAchievementRateItem item)
        {
            item.Level = "Two";
            source.ProviderStandard.AchievementRates = new List<GetAchievementRateItem>
            {
                item
            };
            var response = new GetProviderCourseItem().Map(source, 2, true);

            using (new AssertionScope())
            {
                response.OverallCohort.Should().Be(item.OverallCohort);
                response.OverallAchievementRate.Should().Be(item.OverallAchievementRate);
            }
        }

        [Test, AutoData]
        public void Then_Maps_National_AchievementRates_Mapping_With_Level(GetTrainingCourseProviderResult source, GetAchievementRateItem item2, GetAchievementRateItem expected)
        {
            expected.Level = "Two";
            source.OverallAchievementRates = new List<GetAchievementRateItem>
            {
                item2,
                expected
            };

            var response = new GetProviderCourseItem().Map(source, 2, true);

            using (new AssertionScope())
            {
                response.NationalOverallCohort.Should().Be(expected.OverallCohort);
                response.NationalOverallAchievementRate.Should().Be(expected.OverallAchievementRate);
            }
        }

        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Returning_Null_For_AchievementRate_Data_If_No_AchievementRates(GetTrainingCourseProviderResult source, GetAchievementRateItem item, GetAchievementRateItem item2)
        {
            source.ProviderStandard.AchievementRates = new List<GetAchievementRateItem>();

            source.OverallAchievementRates = new List<GetAchievementRateItem>
            {
                item,
                item2
            };

            var response = new GetProviderCourseItem().Map(source, 1, true);

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