using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;

public class GetHasPermissionRequest(long? ukPrn, long? accountLegalEntityId, Operation operation) : IGetApiRequest
{
    public string GetUrl => $"permissions/has?ukprn={ukPrn}&accountLegalEntityId={accountLegalEntityId}&operation={(int)operation}";
}