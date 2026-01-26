using System;

namespace SFA.DAS.Recruit.Domain;

public class EmailEnvironmentHelper
{
    public EmailEnvironmentHelper(string environmentName)
    {
        SuccessfulApplicationEmailTemplateId =
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "b648c047-b2e3-4ebe-b9b4-a6cc59c2af94" : "35e8b348-5f26-488a-8165-459522f8189b";
        UnsuccessfulApplicationEmailTemplateId=
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "8387c857-b4f9-4cfb-8c44-df4c2560e446" : "95d7ff0c-79fc-4585-9fff-5e583b478d23";
        ApplicationReviewSharedEmailTemplatedId =
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "53058846-e369-4396-87b2-015c9d16360a" : "f6fc57e6-7318-473d-8cb5-ca653035391a";
        AdvertApprovedByDfeTemplateId =
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "c35e76e7-303b-4b18-bb06-ad98cf68158d" : "c445095e-e659-499b-b2ab-81e321a9b591";
        ApplicationSubmittedTemplateId =
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase)
                ? "e07a6992-4d17-4167-b526-2ead6fe9ad4d" : "8aedd294-fd12-4b77-b4b8-2066744e1fdc";
        ProviderAddedToEmployerVacancy =
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase)
                ? "ce5df943-09ad-4f6c-b10a-42584f88046d" : "b00a94c3-4b6e-48df-b28b-a768600fe7a5";
        SharedApplicationsReturned = environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase)
            ? "2f1b70d4-c722-4815-85a0-80a080eac642" : "feb4191d-a373-4040-9bc6-93c09d8039b5";


        CandidateApplicationUrl = $"{GetBaseUrl(environmentName)}applications";
        LiveVacancyUrl = $"{GetBaseUrl(environmentName)}apprenticeship/{{0}}";
        NotificationsSettingsProviderUrl = $"{GetRecruitBaseUrlProvider(environmentName)}{{0}}/notifications-manage/";
        NotificationsSettingsEmployerUrl = $"{GetRecruitBaseUrlEmployer(environmentName)}accounts/{{0}}/notifications-manage/";
        ReviewVacancyReviewInRecruitEmployerUrl = $"{GetRecruitBaseUrlEmployer(environmentName)}accounts/{{0}}/vacancies/{{1}}/check-answers/";
        ReviewVacancyReviewInRecruitProviderUrl = $"{GetRecruitBaseUrlProvider(environmentName)}{{0}}/vacancies/{{1}}/check-your-answers/";
        ApplicationReviewSharedEmployerUrl = $"{GetRecruitBaseUrlEmployer(environmentName)}accounts/{{0}}/vacancies/{{1}}/applications/{{2}}/?vacancySharedByProvider=True";
        ManageAdvertUrl = $"{GetRecruitBaseUrlEmployer(environmentName)}accounts/{{0}}/vacancies/{{1}}/manage/";
        ManageVacancyProviderUrl = $"{GetRecruitBaseUrlProvider(environmentName)}{{0}}/vacancies/{{1}}/manage/";
    }
    public string CandidateApplicationUrl { get; }
    public string LiveVacancyUrl { get; }
    public string NotificationsSettingsProviderUrl { get; }
    public string NotificationsSettingsEmployerUrl { get; }
    public string ReviewVacancyReviewInRecruitEmployerUrl { get; }
    public string ReviewVacancyReviewInRecruitProviderUrl { get; }
    public string ApplicationReviewSharedEmployerUrl { get; }
    public string ManageVacancyProviderUrl { get; }
    public string SuccessfulApplicationEmailTemplateId { get; }
    public string UnsuccessfulApplicationEmailTemplateId { get; }
    public string ApplicationReviewSharedEmailTemplatedId { get; }

    public string AdvertApprovedByDfeTemplateId { get; }
    public string ApplicationSubmittedTemplateId {  get; }
    public string ManageAdvertUrl { get; }
    public string ProviderAddedToEmployerVacancy { get; }
    public string SharedApplicationsReturned { get; }


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