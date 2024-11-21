namespace SFA.DAS.EmployerPR.Application.Requests.Queries.ValidateRequest;

public class ValidatePermissionsRequestQueryResult
{
    public bool IsRequestValid { get; set; }
    public string? Status { get; set; }
    public bool? HasEmployerAccount { get; set; }
    public bool? HasValidPaye { get; set; }
}
