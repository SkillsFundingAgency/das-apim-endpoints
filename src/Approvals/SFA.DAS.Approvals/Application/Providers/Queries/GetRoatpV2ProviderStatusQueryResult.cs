using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.RoatpV2;

namespace SFA.DAS.Approvals.Application.Providers.Queries;

public class GetRoatpV2ProviderStatusQueryResult(int providerStatusTypeId)
{
    public int ProviderStatusTypeId { get; } = providerStatusTypeId;
}

