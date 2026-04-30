namespace SFA.DAS.RecruitQa.Domain;

public class EmailEnvironmentHelper
{
    public EmailEnvironmentHelper(string environmentName)
    {
        VacancyReviewApprovedEmployerTemplateId=
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "d8855c4f-9ce1-4870-93ff-53e609f59a51" : "9a45ff1d-769d-4be2-96fb-dcf605e0108f";
        VacancyReviewApprovedProviderTemplateId=
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "ee2d7ab3-7ac1-47f8-bc32-86290bda55c9" : "48c9ab9e-5b13-4843-b4d5-ee1caa46cc64";
        VacancyReviewEmployerRejectedByDfeTemplateId = 
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "27acd0e9-96fe-47ec-ae33-785e00a453f8" : "5869140a-2a76-4a7c-b4b9-083d2afc5aa5";
        VacancyReviewProviderRejectedByDfeTemplateId = 
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "872e847b-77f5-44a7-b12e-4a19df969ec1" : "048d93c9-4371-45a3-96c4-3f93241a5908";
        ProviderAddedToEmployerVacancy =
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase)
                ? "ce5df943-09ad-4f6c-b10a-42584f88046d" : "b00a94c3-4b6e-48df-b28b-a768600fe7a5";
        
        CandidateApplicationUrl = $"{GetBaseUrl(environmentName)}applications";
        LiveVacancyUrl = $"{GetBaseUrl(environmentName)}apprenticeship/{{0}}";
        NotificationsSettingsProviderUrl = $"{GetRecruitBaseUrlProvider(environmentName)}{{0}}/notifications-manage/";
        NotificationsSettingsEmployerUrl = $"{GetRecruitBaseUrlEmployer(environmentName)}accounts/{{0}}/notifications-manage/";
        ReviewVacancyReviewInRecruitEmployerUrl = $"{GetRecruitBaseUrlEmployer(environmentName)}accounts/{{0}}/vacancies/{{1}}/check-answers/";
        ReviewVacancyReviewInRecruitProviderUrl = $"{GetRecruitBaseUrlProvider(environmentName)}{{0}}/vacancies/{{1}}/check-your-answers/";
    }
    public string CandidateApplicationUrl { get; }
    public string LiveVacancyUrl { get; }
    public string NotificationsSettingsProviderUrl { get; }
    public string NotificationsSettingsEmployerUrl { get; }
    public string ReviewVacancyReviewInRecruitEmployerUrl { get; }
    public string ReviewVacancyReviewInRecruitProviderUrl { get; }
    public string VacancyReviewApprovedEmployerTemplateId { get; }
    public string VacancyReviewApprovedProviderTemplateId { get; }
    public string VacancyReviewEmployerRejectedByDfeTemplateId { get; }
    public string VacancyReviewProviderRejectedByDfeTemplateId { get; }
    public string ProviderAddedToEmployerVacancy { get; }
    

    private static string GetBaseUrl(string environmentName)
    {
        return environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase)
            ? "https://findapprenticeship.service.gov.uk/"
            : $"https://{environmentName.ToLower()}-findapprenticeship.apprenticeships.education.gov.uk/";
    }
    private static string GetRecruitBaseUrlEmployer(string environmentName)
    {
        return environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase)
            ? "https://recruit.manage-apprenticeships.service.gov.uk/"
            : $"https://recruit.{environmentName.ToLower()}-eas.apprenticeships.education.gov.uk/";
    }
    private static string GetRecruitBaseUrlProvider(string environmentName)
    {
        return environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase)
            ? "https://recruit.providers.apprenticeships.education.gov.uk/"
            : $"https://recruit.{environmentName.ToLower()}-pas.apprenticeships.education.gov.uk/";
    }
}