namespace SFA.DAS.ApprenticeAan.Api.Models.Notifications;

public class CreateNotificationModel
{
    public Guid MemberId { get; set; }
    public int NotificationTemplateId { get; set; }
}
