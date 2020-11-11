using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseProviderListItem : ProviderCourseBase
    {
        public GetTrainingCourseProviderListItem Map(   GetProvidersListItem source, string sectorSubjectArea,
            int level, List<DeliveryModeType> deliveryModes, List<FeedbackRatingType> feedbackRatings, bool hasLocation)
        {
            var achievementRate = GetAchievementRateItem(source.AchievementRates, sectorSubjectArea, level);
            var getDeliveryTypes = FilterDeliveryModes(source.DeliveryTypes);
            var getFeedbackResponse = ProviderFeedbackResponse(source.FeedbackRatings, source.FeedbackAttributes);
            
            if (deliveryModes != null && deliveryModes.Any())
            {
                var isInList = getDeliveryTypes.Where(x=>x.DeliveryModeType!= DeliveryModeType.National)
                    .Select(c=>c.DeliveryModeType).Intersect(deliveryModes.Where(c=>c != DeliveryModeType.National).ToList()).ToList();

                if (isInList.Count != deliveryModes.Count(c=>c != DeliveryModeType.National))
                {
                    return null;
                }
                
                if (deliveryModes.Contains(DeliveryModeType.National))
                {
                    if (getDeliveryTypes.FirstOrDefault(x =>
                        x.National && x.DeliveryModeType == DeliveryModeType.Workplace) == null)
                    {
                        return null;
                    }
                }
            }

            if (feedbackRatings != null && feedbackRatings.Any())
            {
                if (!feedbackRatings.Contains((FeedbackRatingType)getFeedbackResponse.TotalFeedbackRating))
                {
                    return null;
                }
            }

            return new GetTrainingCourseProviderListItem
            {
                Name = source.Name,
                TradingName = source.TradingName,
                ProviderId = source.Ukprn,
                OverallCohort = achievementRate?.OverallCohort,
                OverallAchievementRate = achievementRate?.OverallAchievementRate,
                DeliveryModes = getDeliveryTypes,
                Feedback = getFeedbackResponse,
                HasLocation = hasLocation
            };
        }

        public decimal Score { get ; set ; }
    }
}