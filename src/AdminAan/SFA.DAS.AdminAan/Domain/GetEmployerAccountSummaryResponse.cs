namespace SFA.DAS.AdminAan.Domain;

public class GetEmployerAccountSummaryResponse
{
    public IEnumerable<ApprenticeshipSummary> ApprenticeshipStatusSummaryResponse { get; set; } = Enumerable.Empty<ApprenticeshipSummary>();
}

public record ApprenticeshipSummary(int ActiveCount);
