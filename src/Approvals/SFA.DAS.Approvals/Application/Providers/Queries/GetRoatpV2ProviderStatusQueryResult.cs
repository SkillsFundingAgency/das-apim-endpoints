using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;

namespace SFA.DAS.Approvals.Application.Providers.Queries;

public class GetRoatpV2ProviderStatusQueryResult
{   
    public int ProviderStatusTypeId { get; set; }
   

    public static implicit operator GetRoatpV2ProviderStatusQueryResult(GetProviderStatusResponse source) =>
        new GetRoatpV2ProviderStatusQueryResult
        {
             ProviderStatusTypeId  = source.StatusId,
        };
}

