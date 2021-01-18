using SFA.DAS.ManageApprenticeships.InnerApi.Responses;

namespace SFA.DAS.ManageApprenticeships.Api.Models
{
    public class GetProviderResponse
    {
        public int Ukprn { get; set; }
        public string Name { get; set; }
        public string ContactUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        
        public static implicit operator GetProviderResponse(GetProvidersListItem source)
        {
            return new GetProviderResponse
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