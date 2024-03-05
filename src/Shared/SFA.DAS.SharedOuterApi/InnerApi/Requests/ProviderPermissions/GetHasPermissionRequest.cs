using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;

public class GetHasPermissionRequest(long? ukPrn, long? accountLegalEntityId, string operation) : IGetApiRequest
{
    public string GetUrl => $"permissions/has?ukprn={ukPrn}&accountLegalEntityId={accountLegalEntityId}&operation={operation}";
}