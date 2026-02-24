using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
public class GetProvidersResponse
{
    public IEnumerable<Provider> RegisteredProviders { get; set; }
    public int TotalCount { get; set; }
}
