using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetProviderAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public decimal? DistanceInMiles { get; set; }
        
        public GetProviderAddress Map(GetProviderStandardItemAddress source, bool hasLocation)
        {
            return new GetProviderAddress
            {
                Address1 = source.Address1,
                Address2 = source.Address2,
                Address3 = source.Address3,
                Address4 = source.Address4,
                Town = source.Town,
                Postcode = source.Postcode,
                DistanceInMiles = hasLocation ?  source.DistanceInMiles : (decimal?) null
            };
        }
    }
}