using SFA.DAS.LearnerData.Requests.CommitmentsInner;

namespace SFA.DAS.LearnerData.Responses.CommitmentsInner;

public class ApprovalsResponse
{
    public List<ApprovalsChangeResult> Changes { get; set; } = [];
    public List<ApprovalsPrice> Prices { get; set; } = [];
}

public class ApprovalsChangeResult
{
    public string ChangeType { get; set; } = string.Empty;
    public string ApprovalStatus { get; set; } = string.Empty;
    public string? Reason { get; set; }
}
