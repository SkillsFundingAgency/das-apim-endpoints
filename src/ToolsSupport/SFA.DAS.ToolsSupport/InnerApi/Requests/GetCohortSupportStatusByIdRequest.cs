using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetCohortSupportStatusByIdRequest(long cohortId) : IGetApiRequest
{
    public readonly long CohortId = cohortId;
    public string GetUrl => $"api/cohorts/{CohortId}/support-status";
}
