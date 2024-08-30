using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.Recruit.Domain.EmailTemplates;

public class ApplicationResponseEmailTemplate : EmailTemplateArguments
{
    public ApplicationResponseEmailTemplate(string templateId,string recipientEmail, string firstName, string vacancy, string employer, string city, string postcode)
    {
        TemplateId = templateId;
        RecipientAddress = recipientEmail;
        Tokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancy },
            {"employer", employer },
            {"city", city },
            {"postcode", postcode }
        };
    }
}