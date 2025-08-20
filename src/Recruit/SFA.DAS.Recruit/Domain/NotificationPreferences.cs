using System.Collections.Generic;

namespace SFA.DAS.Recruit.Domain;

public class NotificationPreferences
{
    public List<NotificationPreference> EventPreferences { get; set; } = [];
}

public record NotificationPreference(NotificationTypes Event, string Method, NotificationScope Scope, NotificationFrequency Frequency);