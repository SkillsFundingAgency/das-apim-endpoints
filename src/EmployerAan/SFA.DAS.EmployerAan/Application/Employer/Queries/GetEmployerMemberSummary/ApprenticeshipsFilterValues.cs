namespace SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;

public class ApprenticeshipsFilterValues
{
    public IEnumerable<DateTime> StartDates { get; set; } = Enumerable.Empty<DateTime>();
    public IEnumerable<string> Sectors { get; set; } = Enumerable.Empty<string>();
}
