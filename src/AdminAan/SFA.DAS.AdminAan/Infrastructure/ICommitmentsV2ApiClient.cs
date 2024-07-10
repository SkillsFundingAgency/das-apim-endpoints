using RestEase;
using SFA.DAS.AdminAan.Domain;

namespace SFA.DAS.AdminAan.Infrastructure;

public interface ICommitmentsV2ApiClient : IHealthChecker
{
    [Get("/api/accounts/{employerAccountId}/summary")]
    Task<GetEmployerAccountSummaryResponse> GetEmployerAccountSummary([Path] long employerAccountId, CancellationToken cancellationToken);

    [Get("/api/apprenticeships/filters")]
    Task<GetEmployerApprenticeshipsSummaryResponse> GetEmployerApprenticeshipsSummary([Query] long employerAccountId, CancellationToken cancellationToken);
}
