using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
public class GetProvidersResponse
{
    public IEnumerable<Provider> Providers { get; set; }
}

public class Provider
{
    public long Ukprn { get; set; }
    public string Name { get; set; }
}

