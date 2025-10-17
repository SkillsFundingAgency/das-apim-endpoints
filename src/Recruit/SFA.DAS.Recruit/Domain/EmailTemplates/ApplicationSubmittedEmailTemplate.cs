using SFA.DAS.SharedOuterApi.Models.Messages;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Domain.EmailTemplates
{
    public class ApplicationSubmittedEmailTemplate : EmailTemplateArguments
    {
        public ApplicationSubmittedEmailTemplate(
            string templateId,
            string recipientEmail,
            string advertTitle,
            string firstName,
            string vacancyReference,
            string employerName,
            string location,
            string manageAdvertUrl,
            string notificationSettingsUrl) 
        { 
            TemplateId = templateId;
            RecipientAddress = recipientEmail;
            Tokens = new Dictionary<string, string>
            {
                {"advertTitle", advertTitle},
                {"firstName", firstName},
                {"employerName", employerName},
                {"manageAdvertURL", manageAdvertUrl},
                {"notificationSettingsURL", notificationSettingsUrl},
                {"VACcode", vacancyReference},
                {"location", location}
            };
        }
    }
}
