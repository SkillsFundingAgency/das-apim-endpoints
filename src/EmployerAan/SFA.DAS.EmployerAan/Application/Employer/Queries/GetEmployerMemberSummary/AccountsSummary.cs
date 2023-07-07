namespace SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;

public class AccountsSummary
{
    public List<ApprenticeshipStatusSummaryResponse>? ApprenticeshipStatusSummaryResponse { get; set; }
}
public class ApprenticeshipStatusSummaryResponse
{
    public int ActiveCount { get; set; }
}