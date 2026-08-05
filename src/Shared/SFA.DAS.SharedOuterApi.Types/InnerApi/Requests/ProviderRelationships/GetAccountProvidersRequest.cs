using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;

public class GetAccountProvidersRequest(long accountId) : IGetApiRequest
{
    public string GetUrl => $"accounts/{accountId}/providers";
}