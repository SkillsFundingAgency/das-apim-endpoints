namespace SFA.DAS.Approvals.Api.Models.Apprentices;

public class ConfirmEditApprenticeshipResponse
{
    public long ApprenticeshipId { get; set; }
    public bool NeedReapproval { get; set; }
} 