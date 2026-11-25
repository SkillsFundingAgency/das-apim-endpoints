using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderPermissions;

public class GetEmployerAccountProviderPermissionsRequest(string hashedAccountId) : IGetApiRequest
{
    public string GetUrl => $"accountproviderlegalentities?accountHashedId={hashedAccountId}&operations=1&operations=2";
}