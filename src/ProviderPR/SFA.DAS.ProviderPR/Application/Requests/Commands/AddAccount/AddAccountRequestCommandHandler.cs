using MediatR;
using SFA.DAS.ProviderPR.Common;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Notifications.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.GetAccountTeamMembersWhichReceiveNotificationsResponse;

namespace SFA.DAS.ProviderPR.Application.Requests.Commands.AddAccount;

public class AddAccountRequestCommandHandler(
    IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient,
    IAccountsApiClient<AccountsConfiguration> _accountsApiClient
) : IRequestHandler<AddAccountRequestCommand, AddAccountRequestCommandResult>
{
    public async Task<AddAccountRequestCommandResult> Handle(AddAccountRequestCommand command, CancellationToken cancellationToken)
    {
        var addAccountResponse = await _providerRelationshipsApiRestClient.CreateAddAccountRequest(command, cancellationToken);

        var teamMembersResponse = await _accountsApiClient.GetWithResponseCode<List<TeamMember>>(new GetAccountTeamMembersByInternalAccountIdRequest(command.AccountId));

        if(teamMembersResponse.StatusCode != System.Net.HttpStatusCode.OK || !teamMembersResponse.Body.Any())
        {
            return new AddAccountRequestCommandResult(addAccountResponse.RequestId);
        }

        PostNotificationsCommand notificationCommand = new();

        IReadOnlyList<TeamMember> teamMembers = teamMembersResponse.Body;

        // If the provided EmployerContactEmail is null - then we must run through each account owner and send out the 'AddAccountOwnerInvitation' notification.

        if (string.IsNullOrWhiteSpace(command.EmployerContactEmail))
        {
            foreach (TeamMember ownerMember in teamMembers.Where(t => t.IsAcceptedOwnerWithNotifications()))
            {
                notificationCommand.Notifications.Add(CreateAddAccountOwnerInvitationNotification(command, ownerMember));
            }
        }
        else
        {
            // EmployerContactEmail is not null, therefore we must check for a team member associated with the provided EmployerContactEmail.
            // If a user matches the provided email then we must create a 'AddAccountInformation' notification for this team member.

            TeamMember? associatedTeamMember = teamMembers.FirstOrDefault(a => a.Email == command.EmployerContactEmail);

            if (associatedTeamMember is not null && associatedTeamMember.CanReceiveNotifications)
            {
                notificationCommand.Notifications.Add(CreateAddAccountInformationNotification(command, associatedTeamMember));
            }

            // Where "EmployerContactEmail" is not null and is for an 'Owner' of the employer account, we will send a 'AddAccountInvitation' notification to
            // invite the team member, ONLY if notifications are allowed.

            if (associatedTeamMember is not null && associatedTeamMember.IsAccountOwner())
            {
                if (associatedTeamMember.CanReceiveNotifications)
                {
                    notificationCommand.Notifications.Add(CreateAddAccountInvitationNotification(command, associatedTeamMember, addAccountResponse.RequestId));
                }
            }
            else
            {
                // Alternatively, if the team member is not an account owner,
                // an 'AddAccountOwnerInvitation' notification will be sent to the account Owner(s) that allow notifications.

                foreach (TeamMember ownerMember in teamMembers.Where(t => t.IsAcceptedOwnerWithNotifications()))
                {
                    notificationCommand.Notifications.Add(CreateAddAccountOwnerInvitationNotification(command, ownerMember));
                }
            }
        }

        if (notificationCommand.Notifications.Any())
        {
            await _providerRelationshipsApiRestClient.PostNotifications(notificationCommand, cancellationToken);
        }

        return new(addAccountResponse.RequestId);
    }

    private static NotificationModel CreateAddAccountOwnerInvitationNotification(AddAccountRequestCommand command, TeamMember member)
    {
        return CreateNotification(NotificationConstants.AddAccountOwnerInvitationTemplateName, command, member);
    }

    private static NotificationModel CreateAddAccountInformationNotification(AddAccountRequestCommand command, TeamMember member)
    {
        return CreateNotification(NotificationConstants.AddAccountInformationTemplateName, command, member);
    }

    private static NotificationModel CreateAddAccountInvitationNotification(AddAccountRequestCommand command, TeamMember member, Guid? requestId)
    {
        return CreateNotification(NotificationConstants.AddAccountInvitationTemplateName, command, member, requestId);
    }

    private static NotificationModel CreateNotification(string templateName, AddAccountRequestCommand command, TeamMember member, Guid? requestId = null)
    {
        return new NotificationModel()
        {
            TemplateName = templateName,
            NotificationType = NotificationConstants.EmployerNotificationType,
            Ukprn = command.Ukprn,
            EmailAddress = member.Email,
            Contact = member.Name,
            AccountLegalEntityId = command.AccountLegalEntityId,
            CreatedBy = command.RequestedBy,
            RequestId = requestId
        };
    }
}
