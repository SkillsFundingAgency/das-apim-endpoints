using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseProviderListItem : ProviderCourseBase
    {
        public GetTrainingCourseProviderListItem Map(   GetProvidersListItem source, string sectorSubjectArea,
            int level, List<DeliveryModeType> deliveryModes, List<FeedbackRatingType> employerFeedbackRatings, List<FeedbackRatingType> apprenticeFeedbackRatings, bool hasLocation)
        {
            var achievementRate = GetAchievementRateItem(source.AchievementRates, sectorSubjectArea, level);
            var getDeliveryTypes = FilterDeliveryModes(source.DeliveryTypes);
            var getEmployerFeedbackResponse = EmployerFeedbackResponse(source.EmployerFeedback);
            var getApprenticeFeedbackResponse = ApprenticeFeedbackResponse(source.ApprenticeFeedback);

            if (deliveryModes != null && deliveryModes.Any())
            {
                if (!deliveryModes.Exists(c=>getDeliveryTypes.Select(x=>x.DeliveryModeType).Contains(c)))
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

            if (employerFeedbackRatings != null && employerFeedbackRatings.Any())
            {
                if (!employerFeedbackRatings.Contains((FeedbackRatingType)getEmployerFeedbackResponse.TotalFeedbackRating))
                {
                    return null;
                }
            }

            if(apprenticeFeedbackRatings != null && apprenticeFeedbackRatings.Any())
            {
                if(!apprenticeFeedbackRatings.Contains((FeedbackRatingType)getApprenticeFeedbackResponse.TotalFeedbackRating))
                {
                    return null;
                }
            }

            return new GetTrainingCourseProviderListItem
            {
                Name = source.Name,
                TradingName = source.TradingName,
                ProviderId = source.Ukprn,
                ShortlistId = source.ShortlistId,
                OverallCohort = achievementRate?.OverallCohort,
                OverallAchievementRate = achievementRate?.OverallAchievementRate,
                DeliveryModes = getDeliveryTypes,
                EmployerFeedback = getEmployerFeedbackResponse,
                ApprenticeFeedback = getApprenticeFeedbackResponse,
                HasLocation = hasLocation
            };
        }

        public decimal Score { get ; set ; }
    }
}