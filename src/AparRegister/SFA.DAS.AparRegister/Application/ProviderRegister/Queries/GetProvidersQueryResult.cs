using System.Collections.Generic;
using SFA.DAS.AparRegister.InnerApi.Responses;

namespace SFA.DAS.AparRegister.Application.ProviderRegister.Queries
{
    public class GetProvidersQueryResult
    {
        public IEnumerable<RegisteredProvider> RegisteredProviders { get; set; }
    }
}