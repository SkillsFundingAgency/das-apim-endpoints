namespace SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;

public class AccountsSummary
{
    public IEnumerable<ApprenticeshipStatusSummaryResponse> ApprenticeshipStatusSummaryResponse { get; set; } = Enumerable.Empty<ApprenticeshipStatusSummaryResponse>();
}
public class ApprenticeshipStatusSummaryResponse
{
    public int ActiveCount { get; set; }
}