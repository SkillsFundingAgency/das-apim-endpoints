using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetDraftApprenticeshipRequest : IGetApiRequest
    {
        public readonly long CohortId;
        public readonly long DraftApprenticeshipId;

        public string GetUrl => $"api/cohorts/{CohortId}/draft-apprenticeships/{DraftApprenticeshipId}";

        public GetDraftApprenticeshipRequest(long cohortId, long draftApprenticeshipId)
        {
            CohortId = cohortId;
            DraftApprenticeshipId = draftApprenticeshipId;
        }
    }
}
