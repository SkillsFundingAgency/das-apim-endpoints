using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetCohortByIdRequest : IGetApiRequest
{
    public readonly long CohortId;
    public string GetUrl => $"api/cohorts/{CohortId}";

    public GetCohortByIdRequest(long cohortId)
    {
        CohortId = cohortId;
    }
}
