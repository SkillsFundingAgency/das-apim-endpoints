using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetProvidersListItem
    {
        public int Ukprn { get; set; }
        public string Name { get; set; }
        public IEnumerable<GetAchievementRateItem> AchievementRates { get; set; }
        public IEnumerable<GetDeliveryTypeItem> DeliveryTypes { get; set; }
    }
}