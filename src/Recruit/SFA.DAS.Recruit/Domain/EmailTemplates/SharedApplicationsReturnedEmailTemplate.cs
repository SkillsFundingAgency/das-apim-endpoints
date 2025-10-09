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
            {"VACcode", vacancyReference }, // upcoming stories will change this to VacancyReference
            {"employer", employerName },
            {"vacancyTitle", advertTitle }, // to be replaced with advertTitle in upcoming stories
            {"manageVacancyURL", manageVacancyUrl },
            {"notificationSettingsURL", notificationSettingsUrl }
        };
    }
}
