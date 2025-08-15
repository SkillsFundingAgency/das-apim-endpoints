using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Recruit.Domain;

public class NotificationPreferences
{
    public List<NotificationPreference> EventPreferences { get; set; } = [];
}

public record NotificationPreference(NotificationTypes Event, string Method, NotificationScope Scope, NotificationFrequency Frequency);

public static class NotificationPreferencesExtensions
{
    public static NotificationPreference GetForEvent(this NotificationPreferences notificationPreferences, NotificationTypes eventType)
    {
        return notificationPreferences.EventPreferences.Single(x => x.Event == eventType);
    }
}