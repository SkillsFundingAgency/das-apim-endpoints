namespace SFA.DAS.ToolsSupport.Application.Queries;

public class GetUlnSupportApprenticeshipsQueryResult
{
    public List<ApprovedApprenticeshipUlnSummary> ApprovedApprenticeships { get; set; }
}

public class ApprovedApprenticeshipUlnSummary
{
    public long Id { get; set; }
    public long CohortId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Uln { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Status { get; set; }
}