using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.Recruit.Domain.EmailTemplates;

public class ApplicationResponseUnsuccessfulEmailTemplate : EmailTemplateArguments
{
    public ApplicationResponseUnsuccessfulEmailTemplate(string templateId,string recipientEmail, string firstName, string vacancy, string employer, string city, string postcode, string feedback, string applicationUrl)
    {
        var location = string.IsNullOrEmpty(city) ? postcode :
            string.IsNullOrEmpty(postcode) ? city : $"{city}, {postcode}";
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