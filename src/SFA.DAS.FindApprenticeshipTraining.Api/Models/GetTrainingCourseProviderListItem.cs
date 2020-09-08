using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseProviderListItem : ProviderCourseBase
    {
        public string Name { get ; set ; }

        public int ProviderId { get ; set ; }
        public int? OverallCohort { get ; set ; }
        public decimal? OverallAchievementRate { get ; set ; }
        public List<GetDeliveryType> DeliveryModes { get ; set ; }

        public GetTrainingCourseProviderListItem Map(GetProvidersListItem source, string sectorSubjectArea, int level)
        {
            var achievementRate = GetAchievementRateItem(source.AchievementRates, sectorSubjectArea, level);
            var getDeliveryTypes = FilterDeliveryModes(source.DeliveryTypes);
            
            return new GetTrainingCourseProviderListItem
            {
                Name = source.Name,
                ProviderId = source.Ukprn,
                OverallCohort = achievementRate?.OverallCohort,
                OverallAchievementRate = achievementRate?.OverallAchievementRate,
                DeliveryModes = getDeliveryTypes
            };
        }

        private List<GetDeliveryType> FilterDeliveryModes(IEnumerable<GetDeliveryTypeItem> getDeliveryTypeItems)
        {
            var hasWorkPlace = false;
            var hasDayRelease = false;
            var hasBlockRelease = false;
            var filterDeliveryModes = new List<GetDeliveryType>();

            foreach (var deliveryTypeItem in getDeliveryTypeItems)
            {
                var deliveryTypeItemSplit = deliveryTypeItem.DeliveryModes.Split("|").ToList();

                foreach (var mappedType in deliveryTypeItemSplit.Select(MapDeliveryType))
                {
                    switch (mappedType)
                    {
                        case DeliveryModeType.Workplace when !hasWorkPlace:
                            filterDeliveryModes.Add(new GetDeliveryType
                            {
                                DeliveryModeType = DeliveryModeType.Workplace,
                                DistanceInMiles = deliveryTypeItem.DistanceInMiles
                            });
                            hasWorkPlace = true;
                            break;
                        case DeliveryModeType.BlockRelease when !hasBlockRelease:
                            filterDeliveryModes.Add(new GetDeliveryType
                            {
                                DeliveryModeType = DeliveryModeType.BlockRelease,
                                DistanceInMiles = deliveryTypeItem.DistanceInMiles
                            });
                            hasBlockRelease = true;
                            break;
                        case DeliveryModeType.DayRelease when !hasDayRelease:
                            filterDeliveryModes.Add(new GetDeliveryType
                            {
                                DeliveryModeType = DeliveryModeType.DayRelease,
                                DistanceInMiles = deliveryTypeItem.DistanceInMiles
                            });
                            hasDayRelease = true;
                            break;
                    }
                }

                if (hasBlockRelease && hasDayRelease && hasWorkPlace)
                {
                    break;
                }
            }


            return filterDeliveryModes; 
                
        }
        
        private DeliveryModeType MapDeliveryType(string deliveryType)
        {
            return deliveryType switch
            {
                "100PercentEmployer" => DeliveryModeType.Workplace,
                "DayRelease" => DeliveryModeType.DayRelease,
                "BlockRelease" => DeliveryModeType.BlockRelease,
                _ => default
            };
        }
    }
}