using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates
{
    public class SavedSearchEmailNotificationTemplate : EmailTemplateArguments
    {
        public SavedSearchEmailNotificationTemplate(string templateId,
            string recipientEmail,
            string firstName,
            string newApprenticeships,
            string searchAlertDescriptor,
            string searchUrl,
            string unsubscribeUrl,
            string vacancies,
            string searchParams,
            string showSearchLink)
        {
            TemplateId = templateId;
            RecipientAddress = recipientEmail;
            Tokens = new Dictionary<string, string>
            {
                {"firstName", firstName },
                {"newApprenticeships", newApprenticeships },
                {"searchAlertDescriptor", searchAlertDescriptor },
                {"searchUrl", searchUrl },
                {"unsubscribeLink", unsubscribeUrl },
                {"vacancies", vacancies },
                {"searchParams", searchParams },
                {"showSearchLink", showSearchLink }
            };
        }
    }
}