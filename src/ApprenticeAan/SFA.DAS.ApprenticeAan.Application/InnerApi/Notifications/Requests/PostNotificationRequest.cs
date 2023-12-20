using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Notifications;

public record PostNotificationRequest() : IRequest<GetNotificationResponse>
{
    public Guid MemberId { get; set; }
    public int NotificationTemplateId { get; set; }
}
