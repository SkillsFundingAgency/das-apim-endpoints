namespace SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;

public class EmailEnvironmentHelper(string environmentName)
{
    public string ApplicationReminderEmailTemplateId { get; } = environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
        ? "970d86cf-a80f-4012-81e5-eff719d2f1b0" : "78ce88c5-bc7d-4232-86d9-46b864af23ee";
    
    public string VacancyClosedEarlyTemplateId { get; } = environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
        ? "8eed4437-9b7d-422b-be3b-dd943c64e0b6" : "41d0c41b-e95f-4dd7-b8b5-97c87ccd8141";

    public string SavedSearchEmailNotificationTemplateId { get; } = environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase)
        ? "668d5683-1252-43ac-ba57-8023a97813a3" : "4fa070f9-db70-4763-b3f7-04af5e08b167";

    public string CandidateApplicationUrl { get; } = $"{GetBaseUrl(environmentName)}applications";
    public string VacancyUrl { get; } = $"{GetBaseUrl(environmentName)}vacancies";
    public string SettingsUrl { get; } = $"{GetBaseUrl(environmentName)}settings";
    public string SearchUrl => $"{GetBaseUrl(environmentName)}apprenticeships?sort=AgeAsc";
    public string SavedSearchUnSubscribeUrl => $"{GetBaseUrl(environmentName)}saved-searches/unsubscribe?token=";

    public string VacancyDetailsUrl { get; set; } =
        $"{GetBaseUrl(environmentName)}apprenticeship/{{vacancy-reference}}";

    private static string GetBaseUrl(string environmentName)
    {
        return environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase)
            ? "https://findapprenticeship.service.gov.uk/"
            : $"https://{environmentName.ToLower()}-findapprenticeship.apprenticeships.education.gov.uk/";
    }
}