﻿using RestEase;
using SFA.DAS.EmployerPR.Application.Commands.PostNotifications;
using SFA.DAS.EmployerPR.Application.Commands.PostPermissions;
using SFA.DAS.EmployerPR.Application.Queries.GetEmployerRelationships;
using SFA.DAS.EmployerPR.Application.Queries.GetPermissions;
using SFA.DAS.EmployerPR.Application.Queries.GetRelationships;

namespace SFA.DAS.EmployerPR.Infrastructure;

public interface IProviderRelationshipsApiRestClient
{
    [Get("permissions")]
    [AllowAnyStatusCode]
    Task<Response<GetPermissionsResponse>> GetPermissions([Query] long? ukprn, [Query] long? AccountLegalEntityId, CancellationToken cancellationToken);

    [Get("relationships")]
    Task<GetRelationshipsResponse> GetRelationships([Query] long? ukprn, [Query] int? accountLegalEntityId, CancellationToken cancellationToken);

    [Get("relationships/employeraccount/{AccountHashedId}")]
    Task<GetEmployerRelationshipsResponse> GetEmployerRelationships([Path] string AccountHashedId, [Query] long? Ukprn, [Query] string? AccountlegalentityPublicHashedId, CancellationToken cancellationToken);

    [Post("permissions")]
    Task<PostPermissionsCommandResult> PostPermissions([Body] PostPermissionsCommand command, CancellationToken cancellationToken);

    [Post("notifications")]
    Task PostNotifications([Body] PostNotificationsCommand command, CancellationToken cancellationToken);

    [Delete("permissions")]
    Task RemovePermissions([Query] Guid userRef, [Query] long ukprn, [Query] long accountLegalEntityId, CancellationToken cancellationToken);
}