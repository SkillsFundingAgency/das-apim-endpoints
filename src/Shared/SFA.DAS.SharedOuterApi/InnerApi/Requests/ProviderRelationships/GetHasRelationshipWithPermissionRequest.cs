using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
public class GetHasRelationshipWithPermissionRequest(long? ukPrn, Operation operation) : IGetApiRequest
{
    public string GetUrl => $"permissions/has-relationship-with?ukprn={ukPrn}&operation={(int)operation}";
}
