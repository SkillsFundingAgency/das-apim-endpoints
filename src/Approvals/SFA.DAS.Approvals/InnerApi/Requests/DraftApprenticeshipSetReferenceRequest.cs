using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class DraftApprenticeshipSetReferenceRequest : IPutApiRequest
{
    public long DraftApprenticeshipId { get; set; }

    public long CohortId { get; set; }


    public DraftApprenticeshipSetReferenceRequest(long draftApprenticeshipId, long cohortId)
    {
        DraftApprenticeshipId = draftApprenticeshipId;
        CohortId = cohortId;
    }

    public string PutUrl => $"api/cohorts/{CohortId}/draft-apprenticeships/{DraftApprenticeshipId}/reference";

    public object Data { get; set; }

    public class Body : SaveDataRequest
    {
        public string Reference { get; set; }
        public Party Party { get; set; }
    }
}

