using SFA.DAS.EmployerFinance.Application.Queries.GetProvider;

namespace SFA.DAS.EmployerFinance.Api.Models.Providers
{
    public class GetProviderResponse
    {
        public int Ukprn { get; set; }
        public string Name { get; set; }
        public string ContactUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        
        public static implicit operator GetProviderResponse(GetProviderQueryResult source)
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