namespace SFA.DAS.EmployerPR.InnerApi.Requests;

public class PostNotificationsRequest
{
    public PostNotificationsRequest(NotificationModel[] notifications)
    {
        Notifications = notifications;
    }

    public NotificationModel[] Notifications { get; set; }
}
