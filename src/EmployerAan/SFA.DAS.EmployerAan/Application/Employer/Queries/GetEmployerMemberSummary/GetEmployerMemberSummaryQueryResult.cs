namespace SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;

public class GetEmployerMemberSummaryQueryResult
{
    public int ActiveCount { get; set; }
    public DateTime StartDate { get; set; }
    public List<string>? Sectors { get; set; }
}
