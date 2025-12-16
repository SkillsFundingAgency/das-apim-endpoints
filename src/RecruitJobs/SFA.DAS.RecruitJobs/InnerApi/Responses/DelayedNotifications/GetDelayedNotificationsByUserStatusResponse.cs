using System.Collections.Generic;

namespace SFA.DAS.RecruitJobs.InnerApi.Responses.DelayedNotifications;
public record GetDelayedNotificationsByUserStatusResponse
{
    public List<NotificationEmail> Emails { get; set; }
}