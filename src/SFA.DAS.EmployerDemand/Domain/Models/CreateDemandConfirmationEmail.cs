using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.EmployerDemand.Domain.Models
{
    public class CreateDemandConfirmationEmail : EmailTemplateArguments
    {
        public CreateDemandConfirmationEmail(
            string recipientEmail, 
            string recipientName, 
            string standardName, 
            int standardLevel, 
            string location, 
            int numberOfApprentices,
            string stopSharingUrl)
        {
            TemplateId = EmailConstants.CreateDemandConfirmationTemplateId;
            RecipientAddress = recipientEmail;
            ReplyToAddress = EmailConstants.ReplyToAddress;
            Tokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", recipientName },
                {"AEDApprenticeshipTrainingCourse", $"{standardName} (level {standardLevel})" },
                {"AEDApprenticeshipLocation", location },
                {"AEDNumberOfApprentices", numberOfApprentices > 0 ? numberOfApprentices.ToString() : "Not sure" },
                {"AEDStopSharingURL", stopSharingUrl}
            };
        }
    }
}
