using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.Recruit.Domain.EmailTemplates;

public class ApplicationResponseUnsuccessfulEmailTemplate : EmailTemplateArguments
{
    public ApplicationResponseUnsuccessfulEmailTemplate(string templateId,string recipientEmail, string firstName, string vacancy, string employer, string location, string feedback, string applicationUrl)
    {
        TemplateId = templateId;
        RecipientAddress = recipientEmail;
        Tokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancy },
            {"employer", employer },
            {"location", location },
            {"feedback", feedback },
            {"applicationUrl", applicationUrl },
        };
    }
}