using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.EmployerDemand.Domain.Models
{
    public class VerifyEmployerDemandEmail : EmailTemplateArguments
    {
        public VerifyEmployerDemandEmail(
            string recipientEmail, 
            string employerName, 
            string standardName, 
            int standardLevel, 
            string confirmationLink)
        {
            TemplateId = EmailConstants.VerifyContactEmailEmployerDemandTemplateId;
            RecipientAddress = recipientEmail;
            Tokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", employerName },
                {"AEDApprenticeshipTrainingCourse", $"{standardName} (level {standardLevel})" },
                {"AEDConfirmEmailURL", confirmationLink }
            };
        }
    }
}