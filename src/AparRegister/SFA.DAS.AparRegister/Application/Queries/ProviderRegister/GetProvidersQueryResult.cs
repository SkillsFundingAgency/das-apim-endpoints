using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AparRegister.Application.Queries.ProviderRegister
{
    public class GetProvidersQueryResult
    {
        public IEnumerable<OrganisationResponse> RegisteredProviders { get; set; }
    }
}