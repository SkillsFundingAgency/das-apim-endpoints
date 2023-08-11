using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetApprenticeshipEmailOverlapRequest : IGetApiRequest
    {
        public readonly long CohortId;
        public string GetUrl => $"api/cohorts/{CohortId}/email-overlaps";

        public GetApprenticeshipEmailOverlapRequest(long cohortId)
        {
            CohortId = cohortId;
        }
    }
}
