using RestEase;
using SFA.DAS.ApprenticeAan.Application.Employer.Queries.GetEmployerMemberSummary;
using SFA.DAS.ApprenticeAan.Application.InnerApi.ApprenticeshipsValidate;

namespace SFA.DAS.ApprenticeAan.Infrastructure;

public interface ICommitmentsV2InnerApiClient
{
    [Get("/api/accounts/{employerAccountId}/summary")]
    Task<AccountsSummary?> GetEmployerAccountSummary([Path] long employerAccountId, CancellationToken cancellationToken);

    [Get("/api/apprenticeships/filters")]
    Task<ApprenticeshipsFilterValues?> GetApprenticeshipsSummaryForEmployer([Query] long employerAccountId, CancellationToken cancellationToken);

    [Get("/api/apprenticeships/validate")]
    Task<GetApprenticeshipsValidateResponse> GetApprenticeshipsValidate([Query] string firstName, [Query] string lastName, [Query] string dateOfBirth, CancellationToken cancellationToken);
}