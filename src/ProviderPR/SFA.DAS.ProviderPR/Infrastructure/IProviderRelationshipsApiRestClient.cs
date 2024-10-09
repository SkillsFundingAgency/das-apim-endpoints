﻿using RestEase;
using SFA.DAS.ProviderPR.Application.Requests.Commands.AccountInvitation;
using SFA.DAS.ProviderPR.Application.Requests.Commands.AddAccount;
using SFA.DAS.ProviderPR.Application.Requests.Commands.CreatePermissions;
using SFA.DAS.ProviderPR.InnerApi.Notifications.Commands;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Infrastructure;

public interface IProviderRelationshipsApiRestClient
{
    [Get("/health")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> GetHealth(CancellationToken cancellationToken);

    [Get("providers/{ukprn}/relationships")]
    Task<GetProviderRelationshipsResponse> GetProviderRelationships([Path] long ukprn, [RawQueryString] string queryString, CancellationToken cancellationToken);

    [Post("requests/addaccount")]
    Task<AddAccountRequestCommandResult> CreateAddAccountRequest([Body] AddAccountRequestCommand command, CancellationToken cancellationToken);

    [Post("notifications")]
    Task<AddAccountRequestCommandResult> PostNotifications([Body] PostNotificationsCommand command, CancellationToken cancellationToken);

    [Get("relationships")]
    [AllowAnyStatusCode]
    Task<Response<GetRelationshipResponse>> GetRelationship([Query] long ukprn, [Query] long accountLegalEntityId, CancellationToken cancellationToken);

    [Get("requests/{requestId}")]
    Task<GetRequestResponse?> GetRequest([Path] Guid requestId, CancellationToken cancellationToken);

    [Get("requests")]
    Task<GetRequestResponse?> GetRequest([Query] long ukprn, [Query] string paye, CancellationToken cancellationToken);

    [Post("requests/permission")]
    Task<CreatePermissionRequestCommandResult> CreatePermissionsRequest([Body] CreatePermissionRequestCommand command, CancellationToken cancellationToken);

    [Get("requests")]
    [AllowAnyStatusCode]
    Task<Response<GetRequestByUkprnAndPayeResponse?>> GetRequestByUkprnAndPaye([Query] long ukprn, [Query] string paye, CancellationToken cancellationToken);

    [Get("requests")]
    [AllowAnyStatusCode]
    Task<Response<GetRequestByUkprnAndEmailResponse?>> GetRequestByUkprnAndEmail([Query] long ukprn, [Query] string email, CancellationToken cancellationToken);

    [Post("requests/createaccount")]
    Task<CreateAccountInvitationRequestCommandResult> CreateAccountInvitationRequest([Body] CreateAccountInvitationRequestCommand command, CancellationToken cancellationToken);
}
