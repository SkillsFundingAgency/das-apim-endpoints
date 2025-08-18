using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Recruit.Domain;

public class ProviderNotificationPreferences
{
    private const string Channel = "Email"; 
    
    private static readonly List<NotificationPreference> EmployerDefaults = [
        new (NotificationTypes.ApplicationSubmitted, Channel, NotificationScope.OrganisationVacancies, NotificationFrequency.Daily),
        new (NotificationTypes.VacancyApprovedOrRejected, Channel, NotificationScope.OrganisationVacancies, NotificationFrequency.NotSet),
        new (NotificationTypes.SharedApplicationReviewedByEmployer, Channel, NotificationScope.OrganisationVacancies, NotificationFrequency.NotSet),
        new (NotificationTypes.ProviderAttachedToVacancy, Channel, NotificationScope.OrganisationVacancies, NotificationFrequency.Immediately),
    ];

    public static void UpdateWithDefaults(NotificationPreferences preferences)
    {
        var items = preferences.EventPreferences;
        var defaultsToAdd = EmployerDefaults.Where(x => preferences.EventPreferences.All(y => y.Event != x.Event)).Select(x => x with {});
        items.AddRange(defaultsToAdd);
    }
}