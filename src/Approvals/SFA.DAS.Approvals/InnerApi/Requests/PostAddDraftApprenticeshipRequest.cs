using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostAddDraftApprenticeshipRequest(long cohortId, AddDraftApprenticeshipRequest data) : IPostApiRequest
    {
        public long CohortId { get; } = cohortId;
        public object Data { get; set; } = data;

        public string PostUrl => $"api/cohorts/{CohortId}/draft-apprenticeships";
    }
}
