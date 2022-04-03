using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetDraftApprenticeshipsRequest : IGetApiRequest
    {
        public long CohortId { get; set; }
        public string GetUrl => $"api/cohorts/{CohortId}/draft-apprenticeships";

        public GetDraftApprenticeshipsRequest(long cohortId)
        {
            CohortId = cohortId;
        }
    }
}
