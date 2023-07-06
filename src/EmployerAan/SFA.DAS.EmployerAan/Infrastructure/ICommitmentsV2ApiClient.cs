using RestEase;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;

namespace SFA.DAS.EmployerAan.Infrastructure;

public interface ICommitmentsV2ApiClient
{
    [Get("/api/accounts/{EmployerAccountId}/summary")]
    [AllowAnyStatusCode]
    Task<Response<GetEmployerMemberSummaryQueryResult?>> GetEmployerAccounts([Path] int employerAccountId, CancellationToken cancellationToken);

    [Get("/api/apprenticeships/filters")]
    [AllowAnyStatusCode]
    Task<Response<GetEmployerMemberSummaryQueryResult?>> GetApprenticeshipsSummaryForEmployer([Query] int employerAccountId, CancellationToken cancellationToken);
}
