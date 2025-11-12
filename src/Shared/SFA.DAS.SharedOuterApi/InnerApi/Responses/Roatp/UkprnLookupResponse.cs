using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
public class UkprnLookupResponse
{
    public bool Success { get; set; }
    public List<ProviderDetails> Results { get; set; }
}