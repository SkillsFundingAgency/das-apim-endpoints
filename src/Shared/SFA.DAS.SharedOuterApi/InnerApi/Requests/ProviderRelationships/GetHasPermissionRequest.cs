using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;

public class GetHasPermissionRequest(long? ukPrn, long? accountLegalEntityId, Operation operation) : IGetApiRequest
{
    public string GetUrl => $"permissions/has?ukprn={ukPrn}&accountLegalEntityId={accountLegalEntityId}&operation={(int)operation}";
}