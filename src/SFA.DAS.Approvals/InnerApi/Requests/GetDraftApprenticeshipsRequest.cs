using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetDraftApprenticeshipsRequest : IGetApiRequest
    {
        public readonly long CohortId;
        public string GetUrl => $"api/cohorts/{CohortId}/draft-apprenticeships";

        public GetDraftApprenticeshipsRequest(long cohortId)
        {
            CohortId = cohortId;
        }
    }
}
