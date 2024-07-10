namespace SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;

public class EmailEnvironmentHelper(string environmentName)
{
    public string ApplicationReminderEmailTemplateId { get; } = environmentName.Equals("PROD", StringComparison.CurrentCultureIgnoreCase) 
        ? "970d86cf-a80f-4012-81e5-eff719d2f1b0" : "78ce88c5-bc7d-4232-86d9-46b864af23ee";
    
    public string VacancyClosedEarlyTemplateId { get; } = environmentName.Equals("PROD", StringComparison.CurrentCultureIgnoreCase) 
        ? "8eed4437-9b7d-422b-be3b-dd943c64e0b6" : "41d0c41b-e95f-4dd7-b8b5-97c87ccd8141";

    public string CandidateApplicationUrl { get; } = $"{GetBaseUrl(environmentName)}applications";
    public string VacancyUrl { get; } = $"{GetBaseUrl(environmentName)}vacancies";
    public string SettingsUrl { get; } = $"{GetBaseUrl(environmentName)}settings";

    private static string GetBaseUrl(string environmentName)
    {
        return environmentName.Equals("PROD", StringComparison.CurrentCultureIgnoreCase)
            ? "https://findapprenticeship.service.gov.uk/"
            : $"https://{environmentName.ToLower()}-findapprenticeship.apprenticeships.education.gov.uk/";
    }
}