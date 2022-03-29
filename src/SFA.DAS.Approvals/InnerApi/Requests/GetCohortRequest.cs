using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetCohortRequest : IGetApiRequest
    {
        public long _cohortId;
        public string GetUrl => $"api/cohorts/{_cohortId}";

        public GetCohortRequest(long cohortId)
        {
            _cohortId = cohortId;
        }
    }
}
