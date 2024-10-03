using System;

namespace SFA.DAS.Recruit.Domain;

public class EmailEnvironmentHelper
{
    public EmailEnvironmentHelper(string environmentName)
    {
        SuccessfulApplicationEmailTemplateId =
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "7bc576f8-7bf5-4699-8a5b-6be8df7c1e50" : "5b3a4259-286a-41f7-b593-dfc9b9fcd6f9";
        UnsuccessfulApplicationEmailTemplateId=
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "0ad70535-c9d6-4b2e-ad17-d73e023d3387" : "9beb67ac-93c9-4704-ada9-6e417d29356b";
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