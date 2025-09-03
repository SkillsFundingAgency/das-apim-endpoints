using SFA.DAS.SharedOuterApi.Models.Messages;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Domain.EmailTemplates;
public class SharedApplicationsReturnedEmailTemplate : EmailTemplateArguments
{
    public SharedApplicationsReturnedEmailTemplate(string templateId,
        string recipientEmail,
        string advertTitle,
        string firstName,
        string vacancyReference,
        string employerName,
        string manageVacancyUrl,
        string notificationSettingsUrl)
    {
        TemplateId = templateId;
        RecipientAddress = recipientEmail;
        Tokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancyReference },
            {"employer", employerName },
            {"advertTitle", advertTitle },
            {"manageVacancyUrl", manageVacancyUrl },
            {"notificationSettingsUrl", notificationSettingsUrl }
        };
    }
}
