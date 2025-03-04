using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.Recruit.Domain.EmailTemplates;

public class ApplicationResponseSuccessEmailTemplate : EmailTemplateArguments
{
    public ApplicationResponseSuccessEmailTemplate(string templateId,string recipientEmail, string firstName, string vacancy, string employer, string location)
    {
        TemplateId = templateId;
        RecipientAddress = recipientEmail;
        Tokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancy },
            {"employer", employer },
            {"location", location }
        };
    }
}