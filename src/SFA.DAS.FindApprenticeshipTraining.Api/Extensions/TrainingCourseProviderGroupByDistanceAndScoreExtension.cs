using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Extensions
{
    public static class TrainingCourseProviderGroupByDistanceAndScoreExtension
    {
        private static DeliveryModeType _filteredDeliveryMode;

        public static IEnumerable<GetTrainingCourseProviderListItem> OrderByScoreAndDistance(
            this IEnumerable<GetTrainingCourseProviderListItem> source,
            DeliveryModeType requestDeliveryModes = DeliveryModeType.NotFound)
        {
            _filteredDeliveryMode = requestDeliveryModes;
            var returnList = new List<GetTrainingCourseProviderListItem>();
            var getTrainingCourseProviderListItems = source.ToList();

            returnList.AddRange(AddFilteredProviders(getTrainingCourseProviderListItems, returnList, GetProvidersUnderFiveMilesWithScoreGreaterThanOrEqualToSix));
            returnList.AddRange(AddFilteredProviders(getTrainingCourseProviderListItems, returnList, GetProvidersWithinFiveToTenMilesWithScoreGreaterThanOrEqualToSix));
            returnList.AddRange(AddFilteredProviders(getTrainingCourseProviderListItems, returnList, GetProvidersWithinTenMilesWithScoreLessThanSix));
            returnList.AddRange(AddFilteredProviders(getTrainingCourseProviderListItems, returnList, GetProvidersWithinTenToUnderFifteenMiles));
            returnList.AddRange(AddFilteredProviders(getTrainingCourseProviderListItems, returnList, GetProvidersOverFifteenMiles));
            
            return returnList.ToList();
        }

        private static IEnumerable<GetTrainingCourseProviderListItem> AddFilteredProviders(
            IEnumerable<GetTrainingCourseProviderListItem> scoredAndSortedProviders, 
            IEnumerable<GetTrainingCourseProviderListItem> returnList, 
            Func<GetTrainingCourseProviderListItem, bool> providerFilter)
        {
            return scoredAndSortedProviders
                .Except(returnList)
                .Where(providerFilter)
                .OrderByDescending(c=>c.Score)
                .ThenBy(c=>GetDeliveryModeDistance(c.DeliveryModes))
                .ThenByDescending(c=>c.OverallAchievementRate)
                .ThenByDescending(c=>c.OverallCohort)
                .ThenByDescending(c=>c.Feedback.TotalEmployerResponses)
                .ThenBy(c=>c.Name);
        }

        private static decimal GetDeliveryModeDistance(List<GetDeliveryType> deliveryTypes)
        {
            
            if (deliveryTypes.All(delMode => delMode.DeliveryModeType == DeliveryModeType.Workplace))
            {
                return 0;
            }
            var distanceInMiles = deliveryTypes
                .Where(FilterDeliveryType)
                .OrderBy(x=>x.DistanceInMiles)
                .FirstOrDefault()?.DistanceInMiles;
            return distanceInMiles ?? 0;
        
        }

        private static bool FilterDeliveryType(GetDeliveryType deliveryType)
        {
            return deliveryType.DeliveryModeType != DeliveryModeType.Workplace 
                   && (_filteredDeliveryMode == DeliveryModeType.NotFound || deliveryType.DeliveryModeType == _filteredDeliveryMode);
        }

        private static bool GetProvidersUnderFiveMilesWithScoreGreaterThanOrEqualToSix(GetTrainingCourseProviderListItem listItem)
        {
            return listItem.Score >= 6 
                   && GetDeliveryModeDistance(listItem.DeliveryModes) < 5 ;
        }

        private static bool GetProvidersWithinFiveToTenMilesWithScoreGreaterThanOrEqualToSix(GetTrainingCourseProviderListItem listItem)
        {
            return listItem.Score >= 6 
                   && listItem.DeliveryModes
                       .Where(c=>c.DeliveryModeType!=DeliveryModeType.Workplace)
                       .Any(deliveryType=> 
                           deliveryType.DistanceInMiles >= 5 
                           && deliveryType.DistanceInMiles < 10);
        }

        private static bool GetProvidersWithinTenMilesWithScoreLessThanSix(GetTrainingCourseProviderListItem listItem)
        {
            return listItem.Score < 6
                   && GetDeliveryModeDistance(listItem.DeliveryModes) < 10 ;
        }

        private static bool GetProvidersWithinTenToUnderFifteenMiles(GetTrainingCourseProviderListItem listItem)
        {
            return listItem.DeliveryModes
                .Where(c=>c.DeliveryModeType!=DeliveryModeType.Workplace)
                .Any(deliveryType =>  deliveryType.DistanceInMiles >= 10 
                                      && deliveryType.DistanceInMiles < 15);
        }

        private static bool GetProvidersOverFifteenMiles(GetTrainingCourseProviderListItem listItem)
        {
            return listItem.DeliveryModes
                .Where(c=>c.DeliveryModeType!=DeliveryModeType.Workplace)
                .Any(deliveryType => deliveryType.DistanceInMiles >= 15);
        }
    }
}