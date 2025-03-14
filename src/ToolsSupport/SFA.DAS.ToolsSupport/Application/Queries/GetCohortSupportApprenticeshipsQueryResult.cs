namespace SFA.DAS.ToolsSupport.Application.Queries;

public class GetCohortSupportApprenticeshipsQueryResult
{
    public long CohortId { get; set; }
    public long EmployerAccountId { get; set; }
    public string EmployerAccountName { get; set; }
    public string ProviderName { get; set; }
    public long UkPrn { get; set; }
    public string CohortReference { get; set; }
    public int NoOfApprentices { get; set; }
    public string CohortStatus { get; set; }
    public List<ApprovedApprenticeshipCohortSummary> ApprovedApprenticeships { get; set; }
}

public class ApprovedApprenticeshipCohortSummary
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Uln { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Status { get; set; }
}