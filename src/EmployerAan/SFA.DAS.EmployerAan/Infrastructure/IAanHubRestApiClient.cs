using RestEase;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMember;
using SFA.DAS.EmployerAan.Application.Profiles.Queries.GetProfilesByUserType;
using SFA.DAS.EmployerAan.Application.Regions.Queries.GetRegions;

namespace SFA.DAS.EmployerAan.Infrastructure;

public interface IAanHubRestApiClient
{
    public const string RequestedByMemberId = "X-RequestedByMemberId";

    [Get("/profiles/{userType}")]
    Task<GetProfilesByUserTypeQueryResult> GetProfiles([Path] string userType, CancellationToken cancellationToken);

    [Get("/regions")]
    Task<GetRegionsQueryResult> GetRegions(CancellationToken cancellationToken);

    [Get("/employers/{userRef}")]
    [AllowAnyStatusCode]
    Task<Response<GetEmployerMemberQueryResult?>> GetEmployer([Path] Guid userRef, CancellationToken cancellationToken);
}
