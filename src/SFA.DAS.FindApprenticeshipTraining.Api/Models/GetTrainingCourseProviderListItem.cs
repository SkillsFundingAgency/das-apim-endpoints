using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseProviderListItem : ProviderCourseBase
    {
        public GetTrainingCourseProviderListItem Map(GetProvidersListItem source, string sectorSubjectArea, int level)
        {
            var achievementRate = GetAchievementRateItem(source.AchievementRates, sectorSubjectArea, level);
            var getDeliveryTypes = FilterDeliveryModes(source.DeliveryTypes);
            var getFeedbackResponse = ProviderFeedbackResponse(source.FeedbackRatings);
            
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