using MediatR;
using SFA.DAS.EmployerPR.Application.Notifications.Commands.PostNotifications;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;

namespace SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptPermissionsRequest;

public sealed class AcceptPermissionsRequestCommandHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<AcceptPermissionsRequestCommand, Unit>
{
    public async Task<Unit> Handle(AcceptPermissionsRequestCommand command, CancellationToken cancellationToken)
    {
        await _providerRelationshipsApiRestClient.AcceptPermissionsRequest(
           command.RequestId,
           new AcceptPermissionsRequestModel() { ActionedBy = command.ActionedBy },
           cancellationToken
       );

        await _providerRelationshipsApiRestClient.PostNotifications(
            new PostNotificationsCommand(
                [
                    new NotificationModel()
                    {
                        TemplateName = nameof(PermissionEmailTemplateType.UpdatePermissionAccepted),
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
