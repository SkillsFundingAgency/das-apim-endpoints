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
        CandidateApplicationUrl = $"{GetBaseUrl(environmentName)}applications";
    }
    public string CandidateApplicationUrl { get; }
    public string SuccessfulApplicationEmailTemplateId { get; }
    public string UnsuccessfulApplicationEmailTemplateId { get; }
    private static string GetBaseUrl(string environmentName)
    {
        return environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase)
            ? "https://findapprenticeship.service.gov.uk/"
            : $"https://{environmentName.ToLower()}-findapprenticeship.apprenticeships.education.gov.uk/";
    }
}