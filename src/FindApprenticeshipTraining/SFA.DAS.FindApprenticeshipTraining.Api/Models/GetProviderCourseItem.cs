using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetProviderCourseItem : ProviderCourseBase
    {
        public string Email { get ; set ; }

        public string Phone { get ; set ; }

        public string Website { get ; set ; }
        public string MarketingInfo { get ; set ; }


        public int? NationalOverallCohort { get; set; }

        public decimal? NationalOverallAchievementRate { get ; set ; }
        public GetProviderAddress ProviderAddress { get ; set ; }

        public GetProviderCourseItem Map(GetTrainingCourseProviderResult source, string sectorSubjectArea, int level, bool hasLocation)
        {
            var achievementRate = GetAchievementRateItem(source.ProviderStandard.AchievementRates, sectorSubjectArea, level);
            var nationalRate = GetAchievementRateItem(source.OverallAchievementRates, sectorSubjectArea, level);
            var deliveryModes = FilterDeliveryModes(source.ProviderStandard.DeliveryModels);
            var employerFeedbackResponse = EmployerFeedbackResponse(source.ProviderStandard.EmployerFeedback);
            var apprenticeFeedbackResponse = ApprenticeFeedbackResponse(source.ProviderStandard.ApprenticeFeedback);
                        
            return new GetProviderCourseItem
            {
                ProviderAddress = new GetProviderAddress().Map(source.ProviderStandard.ProviderAddress,hasLocation),
                Website = source.ProviderStandard.StandardInfoUrl,
                Phone = source.ProviderStandard.Phone,
                Email = source.ProviderStandard.Email,
                Name = source.ProviderStandard.Name,
                TradingName = source.ProviderStandard.TradingName,
                MarketingInfo = source.ProviderStandard.MarketingInfo,
                ProviderId = source.ProviderStandard.Ukprn,
                OverallCohort = achievementRate?.OverallCohort,
                NationalOverallCohort = nationalRate?.OverallCohort,
                OverallAchievementRate = achievementRate?.OverallAchievementRate,
                NationalOverallAchievementRate = nationalRate?.OverallAchievementRate,
                DeliveryModes = deliveryModes,
                EmployerFeedback = employerFeedbackResponse,
                ApprenticeFeedback = apprenticeFeedbackResponse,
                ShortlistId = source.ProviderStandard.ShortlistId
            };
        }

        public GetProviderCourseItem Map(InnerApi.Responses.GetShortlistItem shortlistItem)
        {
            var achievementRate = GetAchievementRateItem(shortlistItem.ProviderDetails.AchievementRates, shortlistItem.Course.SectorSubjectAreaTier2Description, shortlistItem.Course.Level);

            var deliveryModes = FilterDeliveryModes(shortlistItem.ProviderDetails.DeliveryTypes);

            var getEmployerFeedbackResponse = EmployerFeedbackResponse(shortlistItem.ProviderDetails.EmployerFeedback);
            var getApprenticeFeedbackResponse = ApprenticeFeedbackResponse(shortlistItem.ProviderDetails.ApprenticeFeedback);
            
            return new GetProviderCourseItem
            {
                ProviderAddress = new GetProviderAddress().Map(shortlistItem.ProviderDetails.ProviderAddress,!string.IsNullOrEmpty(shortlistItem.LocationDescription)),
                Website = shortlistItem.ProviderDetails.StandardInfoUrl,
                Phone = shortlistItem.ProviderDetails.Phone,
                Email = shortlistItem.ProviderDetails.Email,
                Name = shortlistItem.ProviderDetails.Name,
                TradingName = shortlistItem.ProviderDetails.TradingName,
                ProviderId = shortlistItem.ProviderDetails.Ukprn,
                OverallCohort = achievementRate?.OverallCohort,
                OverallAchievementRate = achievementRate?.OverallAchievementRate,
                DeliveryModes = deliveryModes,
                EmployerFeedback = getEmployerFeedbackResponse,
                ApprenticeFeedback = getApprenticeFeedbackResponse
            };
        }
    }
}