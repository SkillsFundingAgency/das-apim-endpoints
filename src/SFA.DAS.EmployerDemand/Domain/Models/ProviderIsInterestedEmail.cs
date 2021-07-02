using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.EmployerDemand.Domain.Models
{
    public class ProviderIsInterestedEmail : EmailTemplateArguments
    {
        public ProviderIsInterestedEmail(
            string recipientEmail, 
            string recipientName,
            string standardName, 
            int standardLevel, 
            string location, 
            int numberOfApprentices,
            string providerName,
            string providerEmail,
            string providerPhone,
            string providerWebsite,
            string fatUrl,
            string stopSharingUrl)
        {
            TemplateId = EmailConstants.ProviderInterestedTemplateId;
            RecipientAddress = recipientEmail;
            ReplyToAddress = EmailConstants.ReplyToAddress;
            Tokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", recipientName },
                {"AEDApprenticeshipTrainingCourse", $"{standardName} (level {standardLevel})" },
                {"AEDApprenticeshipLocation", location },
                {"AEDNumberOfApprentices", numberOfApprentices > 0 ? numberOfApprentices.ToString() : "Not sure" },
                {"AEDProviderName", providerName },
                {"AEDProviderEmail", providerEmail },
                {"AEDProviderTelephone", providerPhone },
                {"AEDProviderWebsite", !string.IsNullOrEmpty(providerWebsite) ? providerWebsite : "-" },
                {"FatURL", !string.IsNullOrEmpty(fatUrl) ? $"You can find out more about this training provider at {fatUrl}" : "---" },
                {"AEDStopSharingURL", stopSharingUrl }
            };
        }
    }
}