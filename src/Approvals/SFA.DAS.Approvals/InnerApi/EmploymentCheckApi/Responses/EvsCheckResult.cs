namespace SFA.DAS.Approvals.InnerApi.EmploymentCheckApi.Responses;

public class EvsCheckResult
{
    public bool? Employed { get; set; }
    public int? CompletionStatus { get; set; }
    public string ErrorCode { get; set; }
}
