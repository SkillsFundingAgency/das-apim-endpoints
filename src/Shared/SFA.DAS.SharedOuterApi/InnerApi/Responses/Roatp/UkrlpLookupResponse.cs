using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
public class UkrlpLookupResponse
{
    public bool Success { get; set; }
    public List<UkrlpProviderDetails> Results { get; set; }
}