using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.Models
{
    public class GetProviderAddress
    {
        public string Address1 { get ; set ; }
        public string Address2 { get ; set ; }
        public string Address3 { get ; set ; }
        public string Address4 { get ; set ; }
        public string Town { get ; set ; }
        public string Postcode { get ; set ; }

        public static implicit operator GetProviderAddress(GetProvidersListItemAddress source)
        {
            return new GetProviderAddress
            {
                Address1 = source?.AddressLine1,
                Address2 = source?.AddressLine2,
                Address3 = source?.AddressLine3,
                Address4 = source?.AddressLine4,
                Town = source?.Town,
                Postcode = source?.Postcode
            };
        }
    }
}