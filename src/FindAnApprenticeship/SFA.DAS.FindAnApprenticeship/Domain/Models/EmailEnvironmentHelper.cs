using System;

namespace SFA.DAS.FindAnApprenticeship.Domain.Models;

public class EmailEnvironmentHelper
{
    public EmailEnvironmentHelper(string environmentName)
    {
        SubmitApplicationEmailTemplateId =
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "a07e4767-6cd6-44d1-8e65-044a83f434ad" : "4b765435-ac6f-4d56-93ab-2f0f52402fb5";
        WithdrawApplicationEmailTemplateId=
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "844b7dd8-c2cf-414c-ae10-45a6b42614b7" : "e0c39593-4eed-46bf-9f3d-09c0cd3b046b";
            
        CandidateApplicationUrl = environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
            ? "https://findapprenticeship.service.gov.uk/applications" : $"https://{environmentName.ToLower()}-findapprenticeship.apprenticeships.education.gov.uk/applications";
    }
    
    public string SubmitApplicationEmailTemplateId { get; }
    public string WithdrawApplicationEmailTemplateId { get; }
    public string CandidateApplicationUrl { get; }
}