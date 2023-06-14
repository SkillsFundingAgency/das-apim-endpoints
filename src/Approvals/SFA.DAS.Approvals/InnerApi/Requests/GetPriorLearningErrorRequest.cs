using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetPriorLearningErrorRequest : IGetApiRequest
    {
        public long CohortId { get; }
        public string GetUrl => $"api/cohorts/{CohortId}/draft-apprenticeships/{DraftApprenticeshipId}/prior-learning-errors";

        public GetPriorLearningErrorRequest(long cohortId)
        {
            CohortId = cohortId;
        }
    }
}