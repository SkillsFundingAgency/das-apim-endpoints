using System;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.Api.Models.ManageUserNotificationPreferences;

public class GetUserNotificationPreferencesByDfeUserIdResponse
{
    public Guid Id { get; set; }
    public string DfeUserId { get; set; }
    public NotificationPreferences NotificationPreferences { get; set; }
}