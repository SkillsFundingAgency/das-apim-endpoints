namespace SFA.DAS.EmployerPR.Application.Commands.PostNotifications;

public class PostNotificationsCommand
{
    public PostNotificationsCommand(NotificationModel[] notifications)
    {
        Notifications = notifications;
    }

    public NotificationModel[] Notifications { get; set; }
}
