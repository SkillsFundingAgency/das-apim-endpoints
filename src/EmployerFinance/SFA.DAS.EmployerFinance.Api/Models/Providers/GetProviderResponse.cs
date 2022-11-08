using SFA.DAS.EmployerFinance.InnerApi.Responses;

namespace SFA.DAS.EmployerFinance.Api.Models
{
    public class GetProviderResponse : ProviderResponse
    {
        public static implicit operator GetProviderResponse(GetProvidersListItem source)
        {
            return new GetProviderResponse
            {
                Ukprn = source.Ukprn,
                Name = source.Name,
                Email = source.Email,
                Phone = source.Phone,
                ContactUrl = source.ContactUrl
            };
        }
    }

    public class ProviderResponse
    {
        public int Ukprn { get; set; }
        public string Name { get; set; }
        public string ContactUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        
        public static implicit operator ProviderResponse(GetProvidersListItem source)
        {
            return new ProviderResponse
            {
                Ukprn =  source.Ukprn,
                Name = source.Name,
                Email = source.Email,
                Phone = source.Phone,
                ContactUrl = source.ContactUrl
            };
        }
    }
}