using MediatR;
using SFA.DAS.ProviderPR.Application.Requests.Commands.AddAccount;
using SFA.DAS.ProviderPR.Common;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Notifications.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.GetAccountTeamMembersWhichReceiveNotificationsResponse;

namespace SFA.DAS.ProviderPR.Application.Requests.Commands.CreatePermissions;

public class CreatePermissionRequestCommandHandler(
    IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient,
    IAccountsApiClient<AccountsConfiguration> _accountsApiClient
) : IRequestHandler<CreatePermissionRequestCommand, CreatePermissionRequestCommandResult>
{
    public async Task<CreatePermissionRequestCommandResult> Handle(CreatePermissionRequestCommand command, CancellationToken cancellationToken)
    {
        var createPermissionsResponse = await _providerRelationshipsApiRestClient.CreatePermissionsRequest(command, cancellationToken);

        var teamMembersResponse = await _accountsApiClient.GetWithResponseCode<List<TeamMember>>(new GetAccountTeamMembersByInternalAccountIdRequest(command.AccountId));

        if (teamMembersResponse.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new InvalidOperationException(teamMembersResponse.ErrorContent);
        }

        if (!teamMembersResponse.Body.Any())
        {
            return new CreatePermissionRequestCommandResult(createPermissionsResponse.RequestId);
        }

        PostNotificationsCommand notificationCommand = new();

        IReadOnlyList<TeamMember> teamMembers = teamMembersResponse.Body;

        foreach (TeamMember owner in teamMembers.Where(t => t.IsAcceptedOwnerWithNotifications()))
        {
            notificationCommand.Notifications.Add(
                CreatePermissionRequestNotification(
                    command, 
                    owner,
                    createPermissionsResponse.RequestId
                )
            );
        }

        if(notificationCommand.Notifications.Any())
        {
            await _providerRelationshipsApiRestClient.PostNotifications(notificationCommand, cancellationToken);
        }

        return new CreatePermissionRequestCommandResult(createPermissionsResponse.RequestId);
    }

    private static NotificationModel CreatePermissionRequestNotification(CreatePermissionRequestCommand command, TeamMember owner, Guid requestId)
    {
        return new NotificationModel()
        {
            TemplateName = NotificationConstants.PermissionRequestInvitationTemplateName,
            NotificationType = NotificationConstants.EmployerNotificationType,
            Ukprn = command.Ukprn,
            EmailAddress = owner.Email,
            Contact = owner.Name,
            AccountLegalEntityId = command.AccountLegalEntityId,
            CreatedBy = command.RequestedBy,
            RequestId = requestId
        };
    }
}
