using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerFinance.Application.Queries.GetProviders;

namespace SFA.DAS.EmployerFinance.Api.Models.Providers
{
    public class GetProvidersResponse 
    {
        public IEnumerable<Provider> Providers { get; set; }

        public class Provider
        {
            public int Ukprn { get; set; }
            public string Name { get; set; }
            public string ContactUrl { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }

        public static implicit operator GetProvidersResponse(GetProvidersQueryResult source)
        {
            return new GetProvidersResponse
            {
                Providers = source.Providers.Select(x => new Provider
                {
                    Ukprn = x.Ukprn,
                    Name = x.Name,
                    Email = x.Email,
                    Phone = x.Phone,
                    ContactUrl = x.ContactUrl
                })
            };
        }
    }
}