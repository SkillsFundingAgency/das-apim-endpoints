using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class DraftApprenticeshipAddEmailRequest : IPostApiRequest
{
    public long DraftApprenticeshipId { get; set; }
    public long CohortId { get; set; }


    public DraftApprenticeshipAddEmailRequest(long draftApprenticeshipId, long cohortId)
    {
        DraftApprenticeshipId = draftApprenticeshipId;
        CohortId = cohortId;
    }

    public string PostUrl => $"api/cohorts/{CohortId}/draft-apprenticeships/{DraftApprenticeshipId}/email";

    public object Data { get; set; }

    public class Body : SaveDataRequest
    {
        public string Email { get; set; }      
        public long CohortId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}

