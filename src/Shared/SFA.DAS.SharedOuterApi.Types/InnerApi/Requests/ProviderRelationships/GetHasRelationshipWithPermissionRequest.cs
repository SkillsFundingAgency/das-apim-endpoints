using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;
public class GetHasRelationshipWithPermissionRequest(long? ukPrn, Operation operation) : IGetApiRequest
{
    public string GetUrl => $"permissions/has-relationship-with?ukprn={ukPrn}&operation={(int)operation}";
}
