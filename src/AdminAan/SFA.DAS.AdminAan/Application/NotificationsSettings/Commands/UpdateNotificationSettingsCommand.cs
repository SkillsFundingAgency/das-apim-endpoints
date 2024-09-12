using MediatR;

namespace SFA.DAS.AdminAan.Application.NotificationsSettings.Commands
{
    public class UpdateNotificationSettingsCommand : IRequest<UpdateNotificationSettingsCommandResult>
    {
        public Guid MemberId { get; set; }
        public bool ReceiveNotifications { get; set; }
    }

    public class UpdateNotificationSettingsCommandHandler : IRequestHandler<UpdateNotificationSettingsCommand, UpdateNotificationSettingsCommandResult>
    {
        public Task<UpdateNotificationSettingsCommandResult> Handle(UpdateNotificationSettingsCommand request, CancellationToken cancellationToken)
        {
            // Implementation will go here
            return Task.FromResult(new UpdateNotificationSettingsCommandResult());
        }
    }

    public class UpdateNotificationSettingsCommandResult
    {
        // Properties for the result will go here
    }
}
