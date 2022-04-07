using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetDraftApprenticeshipsRequest : IGetApiRequest
    {
        private readonly long _cohortId;
        public string GetUrl => $"api/cohorts/{_cohortId}/draft-apprenticeships";

        public GetDraftApprenticeshipsRequest(long cohortId)
        {
            _cohortId = cohortId;
        }
    }
}
