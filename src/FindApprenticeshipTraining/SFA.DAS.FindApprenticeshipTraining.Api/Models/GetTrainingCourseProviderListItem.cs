using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseProviderListItem : ProviderCourseBase
    {
        public GetTrainingCourseProviderListItem Map(
            GetProvidersListItem providerDetails,
            List<DeliveryModeType> deliveryModes,
            List<FeedbackRatingType> employerFeedbackRatings,
            List<FeedbackRatingType> apprenticeFeedbackRatings,
            bool hasLocation)
        {
            var achievementRate = providerDetails.AchievementRates.FirstOrDefault(); //Inner api narrows down on the appropriate rates
            var getDeliveryTypes = FilterDeliveryModes(providerDetails.DeliveryModels);
            var getEmployerFeedbackResponse = EmployerFeedbackResponse(providerDetails.EmployerFeedback);
            var getApprenticeFeedbackResponse = ApprenticeFeedbackResponse(providerDetails.ApprenticeFeedback);

            if (deliveryModes != null && deliveryModes.Any())
            {
                if (!deliveryModes.Exists(c => getDeliveryTypes.Select(x => x.DeliveryModeType).Contains(c)))
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

            if (employerFeedbackRatings != null &&
                employerFeedbackRatings.Any() &&
                !employerFeedbackRatings.Contains((FeedbackRatingType)getEmployerFeedbackResponse.TotalFeedbackRating))
            {
                return null;
            }


            if (apprenticeFeedbackRatings != null &&
                apprenticeFeedbackRatings.Any() &&
                !apprenticeFeedbackRatings.Contains((FeedbackRatingType)getApprenticeFeedbackResponse.TotalFeedbackRating))
            {
                return null;

            }

            return new GetTrainingCourseProviderListItem
            {
                Name = providerDetails.Name,
                TradingName = providerDetails.TradingName,
                ProviderId = providerDetails.Ukprn,
                ShortlistId = providerDetails.ShortlistId,
                OverallCohort = achievementRate?.OverallCohort,
                OverallAchievementRate = achievementRate?.OverallAchievementRate,
                DeliveryModes = getDeliveryTypes,
                EmployerFeedback = getEmployerFeedbackResponse,
                ApprenticeFeedback = getApprenticeFeedbackResponse,
                HasLocation = hasLocation
            };
        }

        public decimal Score { get; set; }
    }
}