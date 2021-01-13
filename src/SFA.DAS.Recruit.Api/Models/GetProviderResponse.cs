using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.Models
{
    public class GetProviderResponse
    {
        public int Ukprn { get; set; }
        public string Name { get; set; }
        public string ContactUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public GetProviderAddress Address { get; set; }
        
        public static implicit operator GetProviderResponse(GetProvidersListItem source)
        {
            return new GetProviderResponse
            {
                Ukprn =  source.Ukprn,
                Name = source.Name,
                Email = source.Email,
                Phone = source.Phone,
                ContactUrl = source.ContactUrl,
                Address = source.Address
            };
        }
    }

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
                Address1 = source.Address1,
                Address2 = source.Address2,
                Address3 = source.Address3,
                Address4 = source.Address4,
                Town = source.Town,
                Postcode = source.Postcode
            };
        }
    }
}