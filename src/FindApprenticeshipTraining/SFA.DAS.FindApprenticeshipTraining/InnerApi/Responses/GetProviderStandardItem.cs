using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetProviderStandardItem
    {
        public int Ukprn { get; set; }
        public string Name { get; set; }
        public string TradingName { get; set; }
        public string MarketingInfo { get; set; }
        public string StandardInfoUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int StandardId { get; set; }
        public Guid? ShortlistId { get; set; }
        public IEnumerable<GetAchievementRateItem> AchievementRates { get; set; } = Enumerable.Empty<GetAchievementRateItem>();
        public IEnumerable<GetDeliveryTypeItem> DeliveryTypes { get; set; }
        public IEnumerable<DeliveryModel> DeliveryModels { get; set; }
        public GetEmployerFeedbackResponse EmployerFeedback { get; set; }
        public GetApprenticeFeedbackResponse ApprenticeFeedback { get; set; }
        public GetProviderStandardItemAddress ProviderAddress { get; set; }
        public decimal? DeliveryModelsShortestDistance => !DeliveryModels.Any() ? null : DeliveryModels.MinBy(x => x.DistanceInMiles)?.DistanceInMiles;
    }
}