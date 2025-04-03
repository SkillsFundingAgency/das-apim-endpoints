namespace SFA.DAS.ToolsSupport.Application.Queries;

public class GetUlnSupportApprenticeshipsQueryResult
{
    public List<ApprovedApprenticeshipUlnSummary> ApprovedApprenticeships { get; set; }
}

public class ApprovedApprenticeshipUlnSummary
{
    public long Id { get; set; }
    public long EmployerAccountId { get; set; }
    public long ProviderId { get; set; }
    public string EmployerName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Uln { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
}