using MediatR;

namespace SFA.DAS.Apprenticeships.Application.Notifications.Handlers;

public class ChangeOfStartDateInitiatedCommandHandler : NotificationCommandBase, IRequest<NotificationResponse>
{
    public string? Initiator { get; set; }
}