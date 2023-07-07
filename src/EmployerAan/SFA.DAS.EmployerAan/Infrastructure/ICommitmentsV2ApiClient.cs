using RestEase;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;

namespace SFA.DAS.EmployerAan.Infrastructure;

public interface ICommitmentsV2ApiClient
{
    [Get("/api/accounts/{employerAccountId}/summary")]
    [AllowAnyStatusCode]
    Task<Response<AccountsSummary?>> GetEmployerAccounts([Path] int employerAccountId, CancellationToken cancellationToken);

    [Get("/api/apprenticeships/filters")]
    [AllowAnyStatusCode]
    Task<Response<ApprenticeshipsFilterValues?>> GetApprenticeshipsSummaryForEmployer([Query] int employerAccountId, CancellationToken cancellationToken);
}