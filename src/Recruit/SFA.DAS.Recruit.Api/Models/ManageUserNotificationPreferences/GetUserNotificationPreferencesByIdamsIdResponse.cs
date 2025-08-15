using System;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.Api.Models.ManageUserNotificationPreferences;

public class GetUserNotificationPreferencesByIdamsIdResponse
{
    public Guid Id { get; set; }
    public string IdamsId { get; set; }
    public NotificationPreferences NotificationPreferences { get; set; }
}