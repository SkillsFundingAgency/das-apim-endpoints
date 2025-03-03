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

        TeamMember? associatedTeamMember = teamMembers.FirstOrDefault(a => a.Email == command.EmployerContactEmail);

        if (string.IsNullOrWhiteSpace(command.EmployerContactEmail) || associatedTeamMember is null || !associatedTeamMember.IsAccountOwner())
        {
            notificationCommand.Notifications.AddRange(CreateNotificationsForAllOwners(teamMembers.Where(t => t.IsAcceptedOwnerWithNotifications()), command, CreateRequestResponse.RequestId));
        }

        if (associatedTeamMember is not null && associatedTeamMember.CanReceiveNotifications)
        {
            var templateName = associatedTeamMember.IsAccountOwner() ? NotificationConstants.AddAccountInvitationTemplateName : NotificationConstants.AddAccountInformationTemplateName;
            notificationCommand.Notifications.Add(CreateNotification(templateName, command, associatedTeamMember, CreateRequestResponse.RequestId));
        }

        if (notificationCommand.Notifications.Any())
        {
            await _providerRelationshipsApiRestClient.PostNotifications(notificationCommand, cancellationToken);
        }

        return new(CreateRequestResponse.RequestId);
    }

    private static List<NotificationModel> CreateNotificationsForAllOwners(IEnumerable<TeamMember> teamMembers, AddAccountRequestCommand command, Guid requestId)
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
