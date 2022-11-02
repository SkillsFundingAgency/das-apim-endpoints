using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetProviderDetailsForCourse
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }

        public string Name { get; set; }
        public string TradingName { get; set; }
        public string ContactUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }

        public string StandardInfoUrl { get; set; }
        public string MarketingInfo { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public decimal? ProviderHeadOfficeDistanceInMiles { get; set; }
        public string DeliveryModes { get; set; }

        public List<GetAchievementRateItem> AchievementRates { get; set; } = new List<GetAchievementRateItem>();

        public List<CouseLocationModel> LocationDetails { get; set; } = new List<CouseLocationModel>();
    }
}
