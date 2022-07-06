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
        public IEnumerable<GetDeliveryTypeItem> DeliveryTypes { get; set; }
        public IEnumerable<GetFeedbackAttributeItem> FeedbackAttributes { get; set; }
        public IEnumerable<GetFeedbackRatingItem> FeedbackRatings { get; set; }
        public GetProviderStandardItemAddress ProviderAddress { get ; set ; }
    }
}