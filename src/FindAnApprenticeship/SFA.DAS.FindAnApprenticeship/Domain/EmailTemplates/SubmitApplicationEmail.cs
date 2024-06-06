using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.FindAnApprenticeship.Domain.EmailTemplates;

public class SubmitApplicationEmail : EmailTemplateArguments
{
    public SubmitApplicationEmail(string templateId,string recipientEmail, string firstName, string vacancy, string employer, string city, string postcode, string applicationUrl)
    {
        TemplateId = templateId;
        RecipientAddress = recipientEmail;
        Tokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancy },
            {"employer", employer },
            {"city", city },
            {"postcode", postcode },
            {"yourApplicationsURL", applicationUrl }
        };
    }
}