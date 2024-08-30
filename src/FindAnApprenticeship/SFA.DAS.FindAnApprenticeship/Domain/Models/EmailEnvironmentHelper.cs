using System;

namespace SFA.DAS.FindAnApprenticeship.Domain.Models;

public class EmailEnvironmentHelper
{
    public EmailEnvironmentHelper(string environmentName)
    {
        SubmitApplicationEmailTemplateId =
            environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
                ? "ce8a2834-83ad-4b84-83a1-099f593cd313" : "4b584d3c-7f56-4fd1-95fd-3099ddcb2ffa";
        
        CandidateApplicationUrl = environmentName.Equals("PRD", StringComparison.CurrentCultureIgnoreCase) 
            ? "https://findapprenticeship.service.gov.uk/applications" : $"https://{environmentName.ToLower()}-findapprenticeship.apprenticeships.education.gov.uk/applications";
    }
    
    public string SubmitApplicationEmailTemplateId { get; }
    public string CandidateApplicationUrl { get; }
}