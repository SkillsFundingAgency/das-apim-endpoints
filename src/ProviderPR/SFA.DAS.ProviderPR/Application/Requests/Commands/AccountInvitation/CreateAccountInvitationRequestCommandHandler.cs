using MediatR;
using SFA.DAS.ProviderPR.Common;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Notifications.Commands;

namespace SFA.DAS.ProviderPR.Application.Requests.Commands.AccountInvitation;

public class CreateAccountInvitationRequestCommandHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<CreateAccountInvitationRequestCommand, CreateAccountInvitationRequestCommandResult>
{
    public async Task<CreateAccountInvitationRequestCommandResult> Handle(CreateAccountInvitationRequestCommand command, CancellationToken cancellationToken)
    {
        var response = await _providerRelationshipsApiRestClient.CreateAccountInvitationRequest(command, cancellationToken);

        PostNotificationsCommand notificationCommand = new();
        notificationCommand.Notifications.Add(
            CreateAccountInvitationRequestNotification(command, response.RequestId)
        );

        await _providerRelationshipsApiRestClient.PostNotifications(notificationCommand, cancellationToken);

        return new (response.RequestId);
    }

    private static NotificationModel CreateAccountInvitationRequestNotification(CreateAccountInvitationRequestCommand command, Guid requestId)
    {
        return new NotificationModel()
        {
            TemplateName = NotificationConstants.CreateAccountInvitationTemplateName,
            NotificationType = NotificationConstants.EmployerNotificationType,
            Ukprn = command.Ukprn,
            EmailAddress = command.EmployerContactEmail,
            Contact = $"{command.EmployerContactFirstName} {command.EmployerContactLastName}",
            CreatedBy = command.RequestedBy,
            RequestId = requestId
        };
    }
}
