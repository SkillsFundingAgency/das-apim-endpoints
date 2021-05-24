using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.EmployerDemand.Domain.Models
{
    public class ProviderIsInterestedEmail : EmailTemplateArguments
    {
        public ProviderIsInterestedEmail(
            string recipientEmail, 
            string recipientName, 
            int standardId,
            string standardName, 
            int standardLevel, 
            string location, 
            int numberOfApprentices,
            int ukprn,
            string providerName,
            string providerEmail,
            string providerPhone,
            string providerWebsite,
            bool providerOffersThisCourse)
        {
            TemplateId = EmailConstants.ProviderInterestedTemplateId;
            RecipientAddress = recipientEmail;
            ReplyToAddress = EmailConstants.ReplyToAddress;
            Subject = "A training provider is interested in offering your apprenticeship training";
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
                {"FatURL", providerOffersThisCourse ? $"https://findapprenticeshiptraining.apprenticeships.education.gov.uk/courses/{standardId}/providers/{ukprn}" : "---" },
                {"AEDStopSharingURL", "" }
            };
        }
    }
}