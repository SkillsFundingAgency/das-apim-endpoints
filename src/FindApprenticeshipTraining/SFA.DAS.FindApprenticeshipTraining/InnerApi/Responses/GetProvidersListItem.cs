using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetProvidersListItem
    {
        public int Ukprn { get; set; }
        public string Name { get; set; }
        public string TradingName { get; set; }
        public Guid? ShortlistId { get; set; }
        public IEnumerable<GetAchievementRateItem> AchievementRates { get; set; }
        
        public IEnumerable<GetDeliveryTypeItem> DeliveryTypes { get; set; }

        public IEnumerable<DeliveryModel> DeliveryModels { get; set; }
        public GetEmployerFeedbackResponse EmployerFeedback { get; set; }
        public GetApprenticeFeedbackResponse ApprenticeFeedback { get; set; }
        public decimal? DeliveryModelsShortestDistance => !DeliveryModels.Any() ? null : DeliveryModels.MinBy(x => x.DistanceInMiles)?.DistanceInMiles;

        public bool? IsApprovedByRegulator { get; set; }
    }
}