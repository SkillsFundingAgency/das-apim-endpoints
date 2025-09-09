using SFA.DAS.SharedOuterApi.Models.Messages;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Domain.EmailTemplates;
public class ProviderAddedToEmployerVacancy : EmailTemplateArguments
{
    public ProviderAddedToEmployerVacancy(string templateId,
        string recipientEmail,
        string firstName,
        string advertTitle,
        string vacancyReference,
        string employerName,
        string employerEmail,
        string location,
        string findAnApprenticeshipAdvertUrl,
        string courseTitle,
        string positions,
        string startDate,
        string duration,
        string notificationSettingsUrl)
    {
        TemplateId = templateId;
        RecipientAddress = recipientEmail;
        Tokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"advertTitle", advertTitle },
            {"VACnumber", vacancyReference },
            {"employer", employerName },
            {"location", location },
            {"applicationUrl", findAnApprenticeshipAdvertUrl },
            {"submitterEmail", employerEmail },
            {"courseTitle", courseTitle },
            {"positions", positions },
            {"startDate", startDate },
            {"duration", duration },
            {"notificationSettingsURL", notificationSettingsUrl },
        };
    }
}