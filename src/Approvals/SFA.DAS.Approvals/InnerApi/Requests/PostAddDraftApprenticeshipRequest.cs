using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostAddDraftApprenticeshipRequest : IPostApiRequest
    {
        public long CohortId { get; }
        public object Data { get; set; }

        public string PostUrl => $"api/cohorts/{CohortId}/draft-apprenticeships";

        public PostAddDraftApprenticeshipRequest(long cohortId, AddDraftApprenticeshipRequest data)
        {
            CohortId = cohortId;
            Data = data;
        }
    }
}
