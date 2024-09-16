using MediatR;
using SFA.DAS.EmployerPR.Application.Notifications.Commands.PostNotifications;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;

namespace SFA.DAS.EmployerPR.Application.Requests.Commands.DeclinedRequest;

public sealed class DeclinePermissionRequestCommandHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<DeclinePermissionRequestCommand, Unit>
{
    public async Task<Unit> Handle(DeclinePermissionRequestCommand command, CancellationToken cancellationToken)
    {
        await _providerRelationshipsApiRestClient.DeclineRequest(
            command.RequestId,
            new DeclinedRequestModel() { ActionedBy = command.ActionedBy },
            cancellationToken
        );

        await _providerRelationshipsApiRestClient.PostNotifications(
            new PostNotificationsCommand(
                [
                    new NotificationModel()
                    {
                        TemplateName = nameof(PermissionEmailTemplateType.UpdatePermissionDeclined),
                        NotificationType = nameof(NotificationType.Provider),
                        RequestId = command.RequestId,
                        CreatedBy = command.ActionedBy
                    }
                ]
            ),
            cancellationToken
        );

        return Unit.Value;
    }
}