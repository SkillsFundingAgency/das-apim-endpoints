using System;
using System.Collections.Generic;

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
        public IEnumerable<GetAchievementRateItem> AchievementRates { get; set; }
        //TODO -- for training course controller endpoint (ukprn, larscode), story CSP-287, the 'DeliveryTypes' has been
        // replaced with 'DeliveryModels. 
        // However the endpoint in Shortlist uses 'GetProviderStandardItem' and currently requires 'DeliveryTypes'
        // until that is also updated to use 'DeliveryModels'
        // This will be addressed in CSP-282
        public IEnumerable<GetDeliveryTypeItem> DeliveryTypes { get; set; }
        public IEnumerable<DeliveryModel> DeliveryModels { get; set; }
        public GetEmployerFeedbackResponse EmployerFeedback { get; set; }
        public GetApprenticeFeedbackResponse ApprenticeFeedback { get; set; }
        public GetProviderStandardItemAddress ProviderAddress { get; set; }
    }
}