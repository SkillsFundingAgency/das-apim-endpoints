namespace SFA.DAS.EmployerAan.Api.Models.Notifications;

public class CreateNotificationModel
{
    public Guid MemberId { get; set; }
    public int NotificationTemplateId { get; set; }
}
