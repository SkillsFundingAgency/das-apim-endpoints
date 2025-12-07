namespace SFA.DAS.Approvals.Api.Models.DraftApprenticeships;

public class AddDraftApprenticeEmailRequest
{
    public string Email { get; set; }

    public long CohortId { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
}


