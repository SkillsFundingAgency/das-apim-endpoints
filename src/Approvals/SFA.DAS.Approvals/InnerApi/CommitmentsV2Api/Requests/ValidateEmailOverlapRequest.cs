using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Text;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;

public record ValidateEmailOverlapRequest : IGetApiRequest
{
   
    public long DraftApprenticeshipId { get; set; }
    public string Email { get; set; }
    public long CohortId { get; set; }

    public string StartDate { get; set; }
    public string EndDate { get; set; }


    public ValidateEmailOverlapRequest(long draftApprenticeshipId, string startDate, string endDate, string email, long cohortId)
    {
        DraftApprenticeshipId = draftApprenticeshipId;
        Email = email;
        CohortId = cohortId;
        StartDate = startDate;
        EndDate = endDate;
    }

    public string GetUrl => $"api/overlapping-training-date-request/{DraftApprenticeshipId}/validateEmailOverlap?CohortId={CohortId}&email={Email}&startDate={StartDate}&endDate={EndDate}"; 

}
