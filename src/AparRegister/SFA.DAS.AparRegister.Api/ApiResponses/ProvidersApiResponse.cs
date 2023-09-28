using System.Collections.Generic;
using System.Linq;
using SFA.DAS.AparRegister.Application.ProviderRegister.Queries;
using SFA.DAS.AparRegister.InnerApi.Responses;

namespace SFA.DAS.AparRegister.Api.ApiResponses
{
    public class ProvidersApiResponse
    {
        public IEnumerable<ProviderApiResponse> Providers { get; set; }
        public static implicit operator ProvidersApiResponse(GetProvidersQueryResult source)
        {
            return new ProvidersApiResponse
            {
                Providers = source.RegisteredProviders.Select(c=>(ProviderApiResponse)c).ToList()
            };
        }
    }

    public class ProviderApiResponse
    {
        public int Ukprn { get; set; }
        public string Name { get; set; }
        public string TradingName { get; set; }
        public long ProviderTypeId { get; set; }
        public long StatusId { get; set; }
        public bool CanAccessApprenticeshipService { get; set; }
        public static implicit operator ProviderApiResponse(RegisteredProvider source)
        {
            return new ProviderApiResponse
            {
                Ukprn = source.Ukprn,
                Name = source.Name,
                TradingName = source.TradingName,
                CanAccessApprenticeshipService = source.CanAccessApprenticeshipService,
                StatusId = source.StatusId,
                ProviderTypeId = source.ProviderTypeId
            };
        }
    }
}