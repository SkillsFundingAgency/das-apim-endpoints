using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostAddPriorLearningDataRequest(
        long cohortId,
        long draftApprenticeshipId,
        AddPriorLearningDataRequest data)
        : IPostApiRequest
    {
        public long CohortId { get; } = cohortId;
        public long DraftApprenticeshipId { get; } = draftApprenticeshipId;
        public object Data { get; set; } = data;
        public string PostUrl => $"api/cohorts/{CohortId}/draft-apprenticeships/{DraftApprenticeshipId}/prior-learning-data";
    }
}