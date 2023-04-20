using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostAddPriorLearningDataRequest : IPostApiRequest
    {
        public long CohortId { get; }
        public long DraftApprenticeshipId { get; }
        public object Data { get; set; }
        public string PostUrl => $"api/cohorts/{CohortId}/draft-apprenticeships/{DraftApprenticeshipId}/prior-learning-data";

        public PostAddPriorLearningDataRequest(long cohortId, long draftApprenticeshipId, AddPriorLearningDataRequest data)
        {
            CohortId = cohortId;
            DraftApprenticeshipId = draftApprenticeshipId;
            Data = data;
        }
    }
}