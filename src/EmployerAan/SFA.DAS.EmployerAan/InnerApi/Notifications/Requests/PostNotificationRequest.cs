using MediatR;
using SFA.DAS.EmployerAan.InnerApi.Notifications.Responses;

namespace SFA.DAS.EmployerAan.Application.InnerApi.Notifications;

public record PostNotificationRequest() : IRequest<GetNotificationResponse>
{
    public Guid MemberId { get; set; }
    public int NotificationTemplateId { get; set; }
}
