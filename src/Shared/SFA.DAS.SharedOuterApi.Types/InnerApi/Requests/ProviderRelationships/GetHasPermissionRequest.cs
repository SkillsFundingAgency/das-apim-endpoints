using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;

public class GetHasPermissionRequest(long? ukPrn, long? accountLegalEntityId, Operation operation) : IGetApiRequest
{
    public string GetUrl => $"permissions/has?ukprn={ukPrn}&accountLegalEntityId={accountLegalEntityId}&operations={(int)operation}";
}