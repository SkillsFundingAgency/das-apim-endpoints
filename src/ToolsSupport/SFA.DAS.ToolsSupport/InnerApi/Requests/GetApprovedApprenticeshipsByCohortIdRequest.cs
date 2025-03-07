using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetApprovedApprenticeshipsByCohortIdRequest : IGetApiRequest
{
    public readonly long CohortId;
    public string GetUrl => $"api/cohorts/{CohortId}/approved-apprenticeships";

    public GetApprovedApprenticeshipsByCohortIdRequest(long cohortId)
    {
        CohortId = cohortId;
    }
}
