using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;

public class SendVacancyClosedEarlyTemplate : EmailTemplateArguments
{
    public SendVacancyClosedEarlyTemplate(string templateId,string recipientEmail, string firstName, string vacancy, string vacancyUrl, string employer, string location, DateTime dateApplicationStarted, string settingsUrl)
    {
        TemplateId = templateId;
        RecipientAddress = recipientEmail;
        Tokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancy },
            {"vacancyUrl", vacancyUrl },
            {"employer", employer },
            {"location", location },
            {"dateApplicationStarted", dateApplicationStarted.ToString("d MMM yyyy") },
            {"settingsUrl", settingsUrl }
        };
    }
}