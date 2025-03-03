using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.AdminAan.Domain;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Application.NotificationsSettings.Commands
{
    public class UpdateNotificationSettingsCommand : IRequest
    {
        public Guid MemberId { get; set; }
        public bool ReceiveNotifications { get; set; }
    }

    public class UpdateNotificationSettingsCommandHandler(IAanHubRestApiClient aanHubApiClient) : IRequestHandler<UpdateNotificationSettingsCommand>
    {
        public async Task Handle(UpdateNotificationSettingsCommand request, CancellationToken cancellationToken)
        {
            var patchModel = new JsonPatchDocument<PatchMemberModel>();
            patchModel.Replace(model => model.ReceiveNotifications, request.ReceiveNotifications);

            await aanHubApiClient.UpdateMember(request.MemberId, patchModel, cancellationToken);
        }
    }
}
