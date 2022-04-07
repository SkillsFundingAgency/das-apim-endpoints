using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetCohortRequest : IGetApiRequest
    {
        public readonly long CohortId;
        public string GetUrl => $"api/cohorts/{CohortId}";

        public GetCohortRequest(long cohortId)
        {
            CohortId = cohortId;
        }
    }
}
