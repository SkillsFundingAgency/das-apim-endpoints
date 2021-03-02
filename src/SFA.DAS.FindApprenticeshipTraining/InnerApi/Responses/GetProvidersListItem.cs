using System;
using System.Collections.Generic;

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
        public IEnumerable<GetFeedbackAttributeItem> FeedbackAttributes { get; set; }
        public IEnumerable<GetFeedbackRatingItem> FeedbackRatings { get; set; }
    }
}