using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.Recruit.Domain.EmailTemplates;

public class VacancySubmittedEmailTemplate: EmailTemplateArguments
{
    public VacancySubmittedEmailTemplate(
        string templateId,
        string recipientEmail,
        string advertTitle,
        string firstName,
        string vacancyReference,
        string employerName,
        string location,
        string notificationSettingsUrl)
    {
        TemplateId = templateId;
        RecipientAddress = recipientEmail;
        Tokens = new Dictionary<string, string>
        {
            {"advertTitle", advertTitle},
            {"firstName", firstName },
            {"employerName", employerName },
            {"notificationSettingsURL", notificationSettingsUrl},
            {"VACcode", vacancyReference},
            {"location", location}
        };
    }
}