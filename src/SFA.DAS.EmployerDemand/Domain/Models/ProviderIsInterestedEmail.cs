using System.Collections.Generic;
using System.Linq;
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
            string fatUrl)
        {
            TemplateId = EmailConstants.ProviderInterestedTemplateId;
            RecipientAddress = recipientEmail;
            ReplyToAddress = EmailConstants.ReplyToAddress;
            Subject = "A training provider is interested in offering your apprenticeship training";
            var formattedProviderEmails = 
                $"mailto:{string.Join(", mailto:", providerEmail.Split(',').Select(s => s.Trim()))}";
            Tokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", recipientName },
                {"AEDApprenticeshipTrainingCourse", $"{standardName} (level {standardLevel})" },
                {"AEDApprenticeshipLocation", location },
                {"AEDNumberOfApprentices", numberOfApprentices > 0 ? numberOfApprentices.ToString() : "Not sure" },
                {"AEDProviderName", providerName },
                {"AEDProviderEmail", formattedProviderEmails },
                {"AEDProviderTelephone", providerPhone },
                {"AEDProviderWebsite", !string.IsNullOrEmpty(providerWebsite) ? providerWebsite : "-" },
                {"FatURL", !string.IsNullOrEmpty(fatUrl) ? fatUrl : "---" },
                {"AEDStopSharingURL", "" }
            };
        }
    }
}