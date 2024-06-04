using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;

public class SendSubmitApplicationEmailReminderTemplate : EmailTemplateArguments
{
    public SendSubmitApplicationEmailReminderTemplate(string templateId,string recipientEmail, string firstName, int daysRemaining, string vacancy, string vacancyUrl,string employer, string city, string postcode, string continueApplicationUrl, DateTime closingDate, string settingsUrl)
    {
        TemplateId = templateId;
        RecipientAddress = recipientEmail;
        Tokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"daysRemaining", daysRemaining.ToString() },
            {"vacancy", vacancy },
            {"vacancyUrl", vacancyUrl },
            {"employer", employer },
            {"city", city },
            {"postcode", postcode },
            {"closingDate", closingDate.ToString("d MMM yyyy") },
            {"continueApplicationLink", continueApplicationUrl },
            {"settingsUrl", settingsUrl }
        };
    }
}