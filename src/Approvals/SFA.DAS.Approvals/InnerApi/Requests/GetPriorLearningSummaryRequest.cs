using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetPriorLearningSummaryRequest : IGetApiRequest
    {
        public long CohortId { get; }
        public long DraftApprenticeshipId { get; }
        public string GetUrl => $"api/cohorts/{CohortId}/draft-apprenticeships/{DraftApprenticeshipId}/prior-learning-summary";

        public GetPriorLearningSummaryRequest(long cohortId, long draftApprenticeshipId)
        {
            CohortId = cohortId;
            DraftApprenticeshipId = draftApprenticeshipId;
        }
    }
}