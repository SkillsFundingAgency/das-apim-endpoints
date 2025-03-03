using MediatR;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.EmployerPR.InnerApi.Requests;

namespace SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptAddAccountRequest;

public sealed class AcceptAddAccountRequestCommandHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<AcceptAddAccountRequestCommand, Unit>
{
    public async Task<Unit> Handle(AcceptAddAccountRequestCommand command, CancellationToken cancellationToken)
    {
        await _providerRelationshipsApiRestClient.AcceptAddAccountRequest(
            command.RequestId,
            new AcceptAddAccountRequestModel(command.ActionedBy),
            cancellationToken
        );

        await _providerRelationshipsApiRestClient.PostNotifications(
            new PostNotificationsRequest(
                [
                    new NotificationModel()
                    {
                        TemplateName = nameof(PermissionEmailTemplateType.AddAccountAccepted),
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
