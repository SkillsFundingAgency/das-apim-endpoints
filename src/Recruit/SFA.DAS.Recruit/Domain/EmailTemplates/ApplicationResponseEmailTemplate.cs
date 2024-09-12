using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.Recruit.Domain.EmailTemplates;

public class ApplicationResponseSuccessEmailTemplate : EmailTemplateArguments
{
    public ApplicationResponseSuccessEmailTemplate(string templateId,string recipientEmail, string firstName, string vacancy, string employer, string city, string postcode)
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
        };
    }
}


public class ApplicationResponseUnsuccessfulEmailTemplate : EmailTemplateArguments
{
    public ApplicationResponseUnsuccessfulEmailTemplate(string templateId,string recipientEmail, string firstName, string vacancy, string employer, string city, string postcode, string feedback, string applicationUrl)
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
            {"feedback", feedback },
            {"applicationUrl", applicationUrl },
        };
    }
}