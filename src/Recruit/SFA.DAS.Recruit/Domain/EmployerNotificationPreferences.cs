using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Recruit.Domain;

public static class EmployerNotificationPreferences
{
    private static readonly List<NotificationPreference> EmployerDefaults = [
            new (NotificationTypes.ApplicationSubmitted, "Email", NotificationScope.OrganisationVacancies, NotificationFrequency.Daily),
            new (NotificationTypes.VacancyApprovedOrRejectedByDfE, "Email", NotificationScope.OrganisationVacancies, NotificationFrequency.Daily),
            new (NotificationTypes.VacancySentForReview, "Email", NotificationScope.OrganisationVacancies, NotificationFrequency.Daily),
        ];

    public static void UpdateWithEmployerDefaults(NotificationPreferences preferences)
    {
        var items = preferences.EventPreferences;
        var defaultsToAdd = EmployerDefaults.Where(x => preferences.EventPreferences.All(y => y.Event != x.Event)).Select(x => x with {});
        items.AddRange(defaultsToAdd);

        // var items = EmployerDefaults.Select(x => x with { }).ToList(); // make sure we clone the defaults, not use their instances
        // items.AddRange(preferences.EventPreferences.Where(x => EmployerDefaults.Any(y => x.Event == y.Event)));
        // preferences.EventPreferences = items;
    }
}