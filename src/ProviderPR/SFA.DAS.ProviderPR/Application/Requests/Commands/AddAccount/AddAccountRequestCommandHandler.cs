using MediatR;
using SFA.DAS.ProviderPR.Common;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Notifications.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using TeamMember = SFA.DAS.SharedOuterApi.InnerApi.Responses.GetAccountTeamMembersWhichReceiveNotificationsResponse.TeamMember;

namespace SFA.DAS.ProviderPR.Application.Requests.Commands.AddAccount;

public class AddAccountRequestCommandHandler(
    IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient,
    IAccountsApiClient<AccountsConfiguration> _accountsApiClient
) : IRequestHandler<AddAccountRequestCommand, AddAccountRequestCommandResult>
{
    public async Task<AddAccountRequestCommandResult> Handle(AddAccountRequestCommand command, CancellationToken cancellationToken)
    {
        var createRequestTask = _providerRelationshipsApiRestClient.CreateAddAccountRequest(command, cancellationToken);
        var getTeamMembersTask = _accountsApiClient.GetWithResponseCode<List<TeamMember>>(new GetAccountTeamMembersByInternalAccountIdRequest(command.AccountId));

        await Task.WhenAll(createRequestTask, getTeamMembersTask);

        AddAccountRequestCommandResult CreateRequestResponse = createRequestTask.Result;
        var teamMembersResponse = getTeamMembersTask.Result;

        if (teamMembersResponse.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new InvalidOperationException(teamMembersResponse.ErrorContent);
        }

        if (!teamMembersResponse.Body.Any())
        {
            return new AddAccountRequestCommandResult(CreateRequestResponse.RequestId);
        }

        PostNotificationsCommand notificationCommand = new();

        IReadOnlyList<TeamMember> teamMembers = teamMembersResponse.Body;

        // If the provided EmployerContactEmail is null - then we must run through each account owner and send out the 'AddAccountOwnerInvitation' notification.

        if (string.IsNullOrWhiteSpace(command.EmployerContactEmail))
        {
            notificationCommand.Notifications.AddRange(GetNotificationsForAllOwners(teamMembers.Where(t => t.IsAcceptedOwnerWithNotifications()), command, CreateRequestResponse.RequestId));
        }
        else
        {
            // EmployerContactEmail is not null, therefore we must check for a team member associated with the provided EmployerContactEmail.
            // If a user matches the provided email then we must create a 'AddAccountInformation' notification for this team member.

            TeamMember? associatedTeamMember = teamMembers.FirstOrDefault(a => a.Email == command.EmployerContactEmail);

            if (associatedTeamMember is not null)
            {
                if (associatedTeamMember.CanReceiveNotifications)
                {
                    if (associatedTeamMember.IsAccountOwner())
                    {
                        notificationCommand.Notifications.Add(CreateNotification(NotificationConstants.AddAccountInvitationTemplateName, command, associatedTeamMember, CreateRequestResponse.RequestId));
                    }
                    else
                    {
                        notificationCommand.Notifications.Add(CreateNotification(NotificationConstants.AddAccountInformationTemplateName, command, associatedTeamMember));

                        notificationCommand.Notifications.AddRange(GetNotificationsForAllOwners(teamMembers.Where(t => t.IsAcceptedOwnerWithNotifications()), command, CreateRequestResponse.RequestId));
                    }
                }
            }
            else
            {
                // Alternatively, if the team member is not an account owner,
                // an 'AddAccountOwnerInvitation' notification will be sent to the account Owner(s) that allow notifications.

                notificationCommand.Notifications.AddRange(GetNotificationsForAllOwners(teamMembers.Where(t => t.IsAcceptedOwnerWithNotifications()), command, CreateRequestResponse.RequestId));
            }
        }

        if (notificationCommand.Notifications.Any())
        {
            await _providerRelationshipsApiRestClient.PostNotifications(notificationCommand, cancellationToken);
        }

        return new(CreateRequestResponse.RequestId);
    }

    private static List<NotificationModel> GetNotificationsForAllOwners(IEnumerable<TeamMember> teamMembers, AddAccountRequestCommand command, Guid requestId)
    {
        List<NotificationModel> notifications = new();
        foreach (TeamMember ownerMember in teamMembers.Where(t => t.IsAcceptedOwnerWithNotifications()))
        {
            notifications.Add(CreateNotification(NotificationConstants.AddAccountOwnerInvitationTemplateName, command, ownerMember, requestId));
        }
        return notifications;
    }

    private static NotificationModel CreateNotification(string templateName, AddAccountRequestCommand command, TeamMember member, Guid? requestId = null)
    {
        return new NotificationModel(
            templateName,
            NotificationConstants.EmployerNotificationType,
            command.Ukprn,
            member.Email,
            member.Name,
            command.AccountLegalEntityId,
            command.RequestedBy,
            requestId);
    }
}
