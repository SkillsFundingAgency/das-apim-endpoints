using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Recruit.Domain;

public static class EmployerNotificationPreferences
{
    private static readonly List<NotificationPreference> EmployerDefaults = [
            new (NotificationTypes.ApplicationSubmitted, "Email", NotificationScope.OrganisationVacancies, NotificationFrequency.Daily),
            new (NotificationTypes.VacancyApprovedOrRejected, "Email", NotificationScope.OrganisationVacancies, NotificationFrequency.NotSet),
            new (NotificationTypes.VacancySentForReview, "Email", NotificationScope.OrganisationVacancies, NotificationFrequency.NotSet),
        ];

    public static void UpdateWithDefaults(NotificationPreferences preferences)
    {
        var items = preferences.EventPreferences;
        var defaultsToAdd = EmployerDefaults.Where(x => preferences.EventPreferences.All(y => y.Event != x.Event)).Select(x => x with {});
        items.AddRange(defaultsToAdd);
    }
}