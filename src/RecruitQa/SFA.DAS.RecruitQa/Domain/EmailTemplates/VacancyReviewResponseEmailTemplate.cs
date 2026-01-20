using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.RecruitQa.Domain.EmailTemplates;

public class VacancyReviewResponseEmailTemplate : EmailTemplateArguments
{
    public VacancyReviewResponseEmailTemplate(
        string templateId,string recipientEmail, 
        string advertTitle, 
        string firstName, 
        string employerName, 
        string findAnApprenticeshipAdvertUrl, 
        string notificationSettingsUrl,
        string vacancyReference,
        string location)
    {
        TemplateId = templateId;
        RecipientAddress = recipientEmail;
        Tokens = new Dictionary<string, string>
        {
            {"advertTitle", advertTitle},
            {"firstName", firstName },
            {"employerName", employerName },
            {"FindAnApprenticeshipAdvertURL", findAnApprenticeshipAdvertUrl},
            {"notificationSettingsURL", notificationSettingsUrl},
            {"VACcode", vacancyReference},
            {"location", location}
        };
    }
}