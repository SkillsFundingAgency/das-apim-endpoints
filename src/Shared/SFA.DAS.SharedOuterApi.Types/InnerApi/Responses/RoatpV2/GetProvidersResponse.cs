namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.RoatpV2;
public class GetProvidersResponse
{
    public IEnumerable<Provider> RegisteredProviders { get; set; }
    public int TotalCount { get; set; }
}
