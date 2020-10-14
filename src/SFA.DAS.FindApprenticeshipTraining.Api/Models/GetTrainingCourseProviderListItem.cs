using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseProviderListItem : ProviderCourseBase
    {
        public GetTrainingCourseProviderListItem Map(   GetProvidersListItem source, string sectorSubjectArea,
            int level, List<DeliveryModeType> deliveryModes, List<FeedbackRatingType> feedbackRatings)
        {
            var achievementRate = GetAchievementRateItem(source.AchievementRates, sectorSubjectArea, level);
            var getDeliveryTypes = FilterDeliveryModes(source.DeliveryTypes);
            var getFeedbackResponse = ProviderFeedbackResponse(source.FeedbackRatings, source.FeedbackAttributes);
            
            if (deliveryModes != null && deliveryModes.Any())
            {
                var isInList = getDeliveryTypes.Select(c=>c.DeliveryModeType).Intersect(deliveryModes).ToList();

                if (isInList.Count != deliveryModes.Count)
                {
                    return null;
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
                ProviderId = source.Ukprn,
                OverallCohort = achievementRate?.OverallCohort,
                OverallAchievementRate = achievementRate?.OverallAchievementRate,
                DeliveryModes = getDeliveryTypes,
                Feedback = getFeedbackResponse
            };
        }
    }
}