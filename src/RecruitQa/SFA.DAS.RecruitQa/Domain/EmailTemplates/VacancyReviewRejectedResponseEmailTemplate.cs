using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.RecruitQa.Domain.EmailTemplates;

public class VacancyReviewRejectedResponseEmailTemplate : EmailTemplateArguments
{
    public VacancyReviewRejectedResponseEmailTemplate(
        string templateId,string recipientEmail, 
        string advertTitle, 
        string firstName, 
        string employerName, 
        string rejectedAdvertUrl, 
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
            {"rejectedAdvertURL", rejectedAdvertUrl},
            {"notificationSettingsURL", notificationSettingsUrl},
            {"VACcode", vacancyReference},
            {"location", location}
        };
    }
}