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
            string keyword,
            string searchUrl,
            string location,
            string selectedCategories,
            string apprenticeshipLevels,
            string unsubscribeUrl,
            string vacanciesList)
        {
            TemplateId = templateId;
            RecipientAddress = recipientEmail;
            Tokens = new Dictionary<string, string>
            {
                {"firstName", firstName },
                {"newApprenticeships", newApprenticeships },
                {"searchAlertDescriptor", searchAlertDescriptor },
                {"keyword", keyword },
                {"searchUrl", searchUrl },
                {"location", location },
                {"selectedCategories", selectedCategories },
                {"apprenticeshipLevels", apprenticeshipLevels },
                {"unsubscribeLink", unsubscribeUrl },
                {"vacanciesList", vacanciesList },
            };
        }
    }
}