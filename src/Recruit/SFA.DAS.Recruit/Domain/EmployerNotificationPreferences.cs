using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Recruit.Domain;

public static class EmployerNotificationPreferences
{
    private static readonly Dictionary<NotificationTypes, NotificationPreference> EmployerDefaults = new()
        {
            { NotificationTypes.ApplicationSubmitted, new NotificationPreference(NotificationTypes.ApplicationSubmitted, "Email", NotificationScope.Default, NotificationFrequency.Never) },
            { NotificationTypes.VacancyApprovedOrRejectedByDfE, new NotificationPreference(NotificationTypes.VacancyApprovedOrRejectedByDfE, "Email", NotificationScope.Default, NotificationFrequency.Never) },
        };

    private static NotificationPreference GetOrDefault(NotificationTypes eventType, List<NotificationPreference> current)
    {
        var typePreference = current.FirstOrDefault(x => x.Event == eventType);
        if (typePreference is not null)
        {
            return typePreference;
        }
        
        if (EmployerDefaults.TryGetValue(eventType, out typePreference))
        {
            return typePreference with { }; // must clone the record
        }

        throw new ArgumentOutOfRangeException($"No Employer default preference for notification type '{eventType}'");
    }
    
    public static void UpdateWithEmployerDefaults(NotificationPreferences preferences)
    {
        var items = new List<NotificationPreference>
        {
            GetOrDefault(NotificationTypes.VacancyApprovedOrRejectedByDfE, preferences.EventPreferences),
            GetOrDefault(NotificationTypes.ApplicationSubmitted, preferences.EventPreferences)
        };
        items.AddRange(preferences.EventPreferences.Where(x => x.Event is not NotificationTypes.VacancyApprovedOrRejectedByDfE and not NotificationTypes.ApplicationSubmitted));
        preferences.EventPreferences = items;
    }
}