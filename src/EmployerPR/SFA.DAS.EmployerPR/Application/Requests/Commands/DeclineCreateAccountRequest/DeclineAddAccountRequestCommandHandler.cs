using MediatR;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclinePermissionsRequest;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.EmployerPR.InnerApi.Requests;

namespace SFA.DAS.EmployerPR.Application.Requests.Commands.DeclineCreateAccountRequest;

public sealed class DeclineCreateAccountRequestCommandHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<DeclineCreateAccountRequestCommand, Unit>
{
    public async Task<Unit> Handle(DeclineCreateAccountRequestCommand command, CancellationToken cancellationToken)
    {
        await _providerRelationshipsApiRestClient.DeclineRequest(
           command.RequestId,
           new DeclinedRequestModel() { ActionedBy = command.ActionedBy },
           cancellationToken
       );

        await _providerRelationshipsApiRestClient.PostNotifications(
            new PostNotificationsRequest(
                [
                    new NotificationModel()
                    {
                        TemplateName = nameof(PermissionEmailTemplateType.CreateAccountDeclined),
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
