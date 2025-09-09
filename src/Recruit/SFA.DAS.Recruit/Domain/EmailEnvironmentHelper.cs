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
        VacancyReviewApprovedEmployerTemplateId=
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "d8855c4f-9ce1-4870-93ff-53e609f59a51" : "9a45ff1d-769d-4be2-96fb-dcf605e0108f";
        VacancyReviewApprovedProviderTemplateId=
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "ee2d7ab3-7ac1-47f8-bc32-86290bda55c9" : "48c9ab9e-5b13-4843-b4d5-ee1caa46cc64";
        VacancyReviewRejectedByDfeTemplateId = 
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "27acd0e9-96fe-47ec-ae33-785e00a453f8" : "5869140a-2a76-4a7c-b4b9-083d2afc5aa5";
        ApplicationReviewSharedEmailTemplatedId =
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "53058846-e369-4396-87b2-015c9d16360a" : "f6fc57e6-7318-473d-8cb5-ca653035391a";
        AdvertApprovedByDfeTemplateId =
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "c35e76e7-303b-4b18-bb06-ad98cf68158d" : "c445095e-e659-499b-b2ab-81e321a9b591";
        ProviderAddedToEmployerVacancy =
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase)
                ? "ce5df943-09ad-4f6c-b10a-42584f88046d" : "b00a94c3-4b6e-48df-b28b-a768600fe7a5";

        CandidateApplicationUrl = $"{GetBaseUrl(environmentName)}applications";
        LiveVacancyUrl = $"{GetBaseUrl(environmentName)}apprenticeship/{{0}}";
        NotificationsSettingsProviderUrl = $"{GetRecruitBaseUrlProvider(environmentName)}{{0}}/notifications-manage/";
        NotificationsSettingsEmployerUrl = $"{GetRecruitBaseUrlEmployer(environmentName)}accounts/{{0}}/notifications-manage/";
        ReviewVacancyReviewInRecruitEmployerUrl = $"{GetRecruitBaseUrlEmployer(environmentName)}accounts/{{0}}/vacancies/{{1}}/check-answers/";
        ReviewVacancyReviewInRecruitProviderUrl = $"{GetRecruitBaseUrlProvider(environmentName)}{{0}}/vacancies/{{1}}/check-answers/";
        ApplicationReviewSharedEmployerUrl = $"{GetRecruitBaseUrlEmployer(environmentName)}accounts/{{0}}/vacancies/{{1}}/applications/{{2}}/?vacancySharedByProvider=True";
    }
    public string CandidateApplicationUrl { get; }
    public string LiveVacancyUrl { get; }
    public string NotificationsSettingsProviderUrl { get; }
    public string NotificationsSettingsEmployerUrl { get; }
    public string ReviewVacancyReviewInRecruitEmployerUrl { get; }
    public string ReviewVacancyReviewInRecruitProviderUrl { get; }
    public string ApplicationReviewSharedEmployerUrl { get; }
    public string SuccessfulApplicationEmailTemplateId { get; }
    public string UnsuccessfulApplicationEmailTemplateId { get; }
    public string VacancyReviewApprovedEmployerTemplateId { get; }
    public string VacancyReviewApprovedProviderTemplateId { get; }
    public string VacancyReviewRejectedByDfeTemplateId { get; }
    public string ApplicationReviewSharedEmailTemplatedId { get; }
    public string AdvertApprovedByDfeTemplateId { get; }
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