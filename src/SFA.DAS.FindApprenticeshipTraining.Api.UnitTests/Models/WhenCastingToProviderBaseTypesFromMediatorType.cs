using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingToProviderBaseTypesFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Matching_AchievementRate(string sectorSubjectArea,
            GetProvidersListItem source, GetAchievementRateItem item, GetAchievementRateItem item2)
        {
            item.SectorSubjectArea = sectorSubjectArea;
            item.Level = "Two";
            source.AchievementRates = new List<GetAchievementRateItem>
            {
               item,
               item2
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,2);

            response.Name.Should().Be(source.Name);
            response.ProviderId.Should().Be(source.Ukprn);
            response.OverallCohort.Should().Be(item.OverallCohort);
            response.OverallAchievementRate.Should().Be(item.OverallAchievementRate);

        }
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Returning_Null_For_AchievementRate_Data_If_No_Matching_No_AchievementRate(string sectorSubjectArea,
            GetProvidersListItem source, GetAchievementRateItem item, GetAchievementRateItem item2)
        {
            source.AchievementRates = new List<GetAchievementRateItem>
            {
                item,
                item2
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.Name.Should().Be(source.Name);
            response.ProviderId.Should().Be(source.Ukprn);
            response.OverallCohort.Should().BeNull();
            response.OverallAchievementRate.Should().BeNull();

        }
        
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Returning_Null_For_AchievementRate_Data_If_No_AchievementRates_And_Empty_List_For_DeliveryModes_If_No_Delivery_Modes(string sectorSubjectArea, GetProvidersListItem source)
        {
            source.AchievementRates = null;
            source.DeliveryTypes = new List<GetDeliveryTypeItem>();
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.Name.Should().Be(source.Name);
            response.ProviderId.Should().Be(source.Ukprn);
            response.OverallCohort.Should().BeNull();
            response.OverallAchievementRate.Should().BeNull();
            response.DeliveryModes.Should().BeEmpty();

        }

        [Test, AutoData]
        public void Then_Maps_Delivery_Types_Returning_The_Smallest_Distance_Only(string sectorSubjectArea, GetProvidersListItem source)
        {
            source.AchievementRates = null;
            source.DeliveryTypes = new List<GetDeliveryTypeItem>
            {
                new GetDeliveryTypeItem
                {
                    DeliveryModes = "100PercentEmployer|DayRelease",
                    DistanceInMiles = 2.5m
                },
                new GetDeliveryTypeItem
                {
                    DeliveryModes = "100PercentEmployer|DayRelease|BlockRelease",
                    DistanceInMiles = 3.1m
                },
                new GetDeliveryTypeItem
                {
                    DeliveryModes = "BlockRelease",
                    DistanceInMiles = 5.5m
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.DeliveryModes.Count.Should().Be(3);
            response.DeliveryModes.FirstOrDefault(c => c.DeliveryModeType == DeliveryModeType.BlockRelease)?.DistanceInMiles.Should().Be(3.1m);
            response.DeliveryModes.FirstOrDefault(c => c.DeliveryModeType == DeliveryModeType.DayRelease)?.DistanceInMiles.Should().Be(2.5m);
            response.DeliveryModes.FirstOrDefault(c => c.DeliveryModeType == DeliveryModeType.Workplace)?.DistanceInMiles.Should().Be(2.5m);
        }
        
        [Test]
        [InlineAutoData("100PercentEmployer",DeliveryModeType.Workplace)]
        [InlineAutoData("BlockRelease",DeliveryModeType.BlockRelease)]
        [InlineAutoData("DayRelease",DeliveryModeType.DayRelease)]
        public void Then_Maps_Delivery_Types_Returning_The_Smallest_Distance_Only_For_One_Type(string deliveryModeString, DeliveryModeType deliveryModeType, string sectorSubjectArea, GetProvidersListItem source)
        {
            source.AchievementRates = null;
            source.DeliveryTypes = new List<GetDeliveryTypeItem>
            {
                new GetDeliveryTypeItem
                {
                    DeliveryModes = deliveryModeString,
                    DistanceInMiles = 2.5m
                },
                new GetDeliveryTypeItem
                {
                    DeliveryModes = deliveryModeString,
                    DistanceInMiles = 3.1m
                },
                new GetDeliveryTypeItem
                {
                    DeliveryModes = deliveryModeString,
                    DistanceInMiles = 5.5m
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.DeliveryModes.Count.Should().Be(1);
            response.DeliveryModes.FirstOrDefault(c => c.DeliveryModeType == deliveryModeType)?.DistanceInMiles.Should().Be(2.5m);
        }

        [Test, AutoData]
        public void Then_Maps_All_DeliveryType_Fields(string sectorSubjectArea, GetProvidersListItem source, GetDeliveryTypeItem item)
        {
            source.AchievementRates = null;
            item.DeliveryModes = "100PercentEmployer";
            source.DeliveryTypes = new List<GetDeliveryTypeItem>{item};
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);
            
            response.DeliveryModes.First().Should().BeEquivalentTo(item, options => options.Excluding(c=>c.DeliveryModes));
        }
    }
}