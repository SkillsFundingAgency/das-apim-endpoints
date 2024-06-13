using RestEase;
using SFA.DAS.EmployerPR.Application.Queries.GetEmployerRelationships;
using SFA.DAS.EmployerPR.Application.Queries.GetPermissions;

namespace SFA.DAS.EmployerPR.Infrastructure;

public interface IProviderRelationshipsApiRestClient
{
    [Get("permissions")]
    Task<GetPermissionsResponse> GetPermissions([Query] long? ukprn, [Query] string? PublicHashedId, CancellationToken cancellationToken);

    [Get("relationships/employeraccount/{AccountHashedId}")]
    Task<GetEmployerRelationshipsResponse> GetEmployerRelationships([Path]string AccountHashedId, [Query]long? Ukprn, [Query]string? AccountlegalentityPublicHashedId, CancellationToken cancellationToken);
}