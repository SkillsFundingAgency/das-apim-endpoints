using SFA.DAS.SharedOuterApi.Models.Messages;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Domain.EmailTemplates;
public class ApplicationReviewSharedEmailTemplate : EmailTemplateArguments
{
    public ApplicationReviewSharedEmailTemplate(string templateId,
        string recipientEmail,
        string firstName,
        string trainingProvider,
        string advertTitle,
        string vacancyReference,
        string applicationUrl)
    {
        TemplateId = templateId;
        RecipientAddress = recipientEmail;
        Tokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"trainingProvider", trainingProvider },
            {"advertTitle", advertTitle },
            {"vacancyReference", vacancyReference },
            {"applicationURL", applicationUrl },
        };
    }
}