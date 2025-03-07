using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetCohortSupportStatusByIdRequest : IGetApiRequest
{
    public readonly long CohortId;
    public string GetUrl => $"api/cohorts/{CohortId}/support-status";

    public GetCohortSupportStatusByIdRequest(long cohortId)
    {
        CohortId = cohortId;
    }
}
