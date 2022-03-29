using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetDraftApprenticeshipsRequest : IGetApiRequest
    {
        public long _cohortId;
        public string GetUrl => $"api/cohorts/{_cohortId}/draft-apprenticeships";

        public GetDraftApprenticeshipsRequest(long cohortId)
        {
            _cohortId = cohortId;
        }
    }
}
