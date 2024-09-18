using MediatR;
using RestEase;
using SFA.DAS.EmployerPR.Application.EmployerRelationships.Queries.GetEmployerRelationships;
using SFA.DAS.EmployerPR.Application.Notifications.Commands.PostNotifications;
using SFA.DAS.EmployerPR.Application.Permissions.Commands.PostPermissions;
using SFA.DAS.EmployerPR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.EmployerPR.Application.Relationships.Queries.GetRelationships;
using SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptAddAccountRequest;
using SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptPermissionsRequest;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclinePermissionsRequest;
using SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;

namespace SFA.DAS.EmployerPR.Infrastructure;

public interface IProviderRelationshipsApiRestClient
{
    [Get("permissions")]
    [AllowAnyStatusCode]
    Task<Response<GetPermissionsResponse>> GetPermissions([Query] long? ukprn, [Query] long? accountLegalEntityId, CancellationToken cancellationToken);

    [Get("relationships")]
    Task<GetRelationshipsResponse> GetRelationships([Query] long? ukprn, [Query] long? accountLegalEntityId, CancellationToken cancellationToken);

    [Get("employers/{accountId}/relationships")]
    Task<GetEmployerRelationshipsResponse> GetEmployerRelationships([Path] long accountId, CancellationToken cancellationToken);

    [Post("permissions")]
    Task<PostPermissionsCommandResult> PostPermissions([Body] PostPermissionsCommand command, CancellationToken cancellationToken);

    [Post("notifications")]
    Task PostNotifications([Body] PostNotificationsCommand command, CancellationToken cancellationToken);

    [Delete("permissions")]
    Task RemovePermissions([Query] Guid userRef, [Query] long ukprn, [Query] long accountLegalEntityId, CancellationToken cancellationToken);

    [Get("requests/{requestId}")]
    Task<GetRequestResponse?> GetRequest([Path] Guid requestId, CancellationToken cancellationToken);

    [Put("requests/{requestId}/declined")]
    Task<Unit> DeclineRequest([Path] Guid requestId, [Body] DeclinedRequestModel model, CancellationToken cancellationToken);

    [Post("requests/{requestId}/permission/accepted")]
    Task<Unit> AcceptPermissionsRequest([Path] Guid requestId, [Body] AcceptPermissionsRequestModel model, CancellationToken cancellationToken);

    [Post("requests/{requestId}/addaccount/accepted")]
    Task<Unit> AcceptAddAccountRequest([Path] Guid requestId, [Body] AcceptAddAccountRequestModel model, CancellationToken cancellationToken);
}