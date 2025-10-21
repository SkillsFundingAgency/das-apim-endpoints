namespace SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;

public class EmailEnvironmentHelper(string environmentName)
{
    public string ApplicationReminderEmailTemplateId { get; } = environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
        ? "a0248572-0d33-46a3-8bb8-560a95cc6e69" : "00d36062-dbba-47b7-8442-10f80f42e127";
    
    public string SavedSearchEmailNotificationTemplateId { get; } = environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase)
        ? "668d5683-1252-43ac-ba57-8023a97813a3" : "4fa070f9-db70-4763-b3f7-04af5e08b167";

    public string CandidateApplicationUrl { get; } = $"{GetBaseUrl(environmentName)}applications";
    public string VacancyUrl { get; } = $"{GetBaseUrl(environmentName)}vacancies";
    public string SettingsUrl { get; } = $"{GetBaseUrl(environmentName)}settings";
    public string SearchUrl => $"{GetBaseUrl(environmentName)}apprenticeships";
    public string SavedSearchUnSubscribeUrl => $"{GetBaseUrl(environmentName)}saved-searches/unsubscribe?id=";

    public string VacancyDetailsUrl { get; set; } =
        $"{GetBaseUrl(environmentName)}apprenticeship/{{vacancy-reference}}";

    private static string GetBaseUrl(string environmentName)
    {
        return environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase)
            ? "https://findapprenticeship.service.gov.uk/"
            : $"https://{environmentName.ToLower()}-findapprenticeship.apprenticeships.education.gov.uk/";
    }
}