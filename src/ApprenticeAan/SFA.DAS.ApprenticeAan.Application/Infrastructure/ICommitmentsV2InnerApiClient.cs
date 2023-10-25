using RestEase;
using SFA.DAS.ApprenticeAan.Application.Employer.Queries.GetEmployerMemberSummary;

namespace SFA.DAS.ApprenticeAan.Infrastructure;

public interface ICommitmentsV2InnerApiClient
{
    [Get("/api/accounts/{employerAccountId}/summary")]
    Task<AccountsSummary?> GetEmployerAccountSummary([Path] long employerAccountId, CancellationToken cancellationToken);

    [Get("/api/apprenticeships/filters")]
    Task<ApprenticeshipsFilterValues?> GetApprenticeshipsSummaryForEmployer([Query] long employerAccountId, CancellationToken cancellationToken);
}