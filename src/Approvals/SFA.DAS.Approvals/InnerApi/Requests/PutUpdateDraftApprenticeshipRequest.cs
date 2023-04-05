using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PutUpdateDraftApprenticeshipRequest : IPutApiRequest
    {
        public long CohortId { get; }
        public long ApprenticeshipId { get; }
        public object Data { get; set; }

        public string PutUrl => $"api/cohorts/{CohortId}/draft-apprenticeships/{ApprenticeshipId}";

        public PutUpdateDraftApprenticeshipRequest(long cohortId, long apprenticeshipId, UpdateDraftApprenticeshipRequest data)
        {
            CohortId = cohortId;
            ApprenticeshipId = apprenticeshipId;
            Data = data;
        }
    }
}
