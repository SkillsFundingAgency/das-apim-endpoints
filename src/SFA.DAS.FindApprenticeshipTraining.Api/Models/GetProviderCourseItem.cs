using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetProviderCourseItem : ProviderCourseBase
    {
        public string Email { get ; set ; }

        public string Phone { get ; set ; }

        public string Website { get ; set ; }

        
        public int? NationalOverallCohort { get; set; }
        
        public decimal? NationalOverallAchievementRate { get ; set ; }
        public GetProviderCourseItem Map(GetTrainingCourseProviderResult source, string sectorSubjectArea, int level)
        {
            var achievementRate = GetAchievementRateItem(source.ProviderStandard.AchievementRates, sectorSubjectArea, level);
            var nationalRate = GetAchievementRateItem(source.OverallAchievementRates, sectorSubjectArea, level);
            var deliveryModes = FilterDeliveryModes(source.ProviderStandard.DeliveryTypes);
            var getFeedbackResponse = ProviderFeedbackResponse(source.ProviderStandard.FeedbackRatings, source.ProviderStandard.FeedbackAttributes);
            
            return new GetProviderCourseItem
            {
                Website = source.ProviderStandard.ContactUrl,
                Phone = source.ProviderStandard.Phone,
                Email = source.ProviderStandard.Email,
                Name = source.ProviderStandard.Name,
                ProviderId = source.ProviderStandard.Ukprn,
                OverallCohort = achievementRate?.OverallCohort,
                NationalOverallCohort = nationalRate?.OverallCohort,
                OverallAchievementRate = achievementRate?.OverallAchievementRate,
                NationalOverallAchievementRate = nationalRate?.OverallAchievementRate,
                DeliveryModes = deliveryModes,
                Feedback = getFeedbackResponse
            };
        }

        
    }
}