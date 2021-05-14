using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.EmployerDemand.Domain.Models
{
    public class CreateVerifyEmployerDemandEmail : EmailTemplateArguments
    {
        public CreateVerifyEmployerDemandEmail(string recipientEmail, string employerName, string standardName, int standardLevel, string confirmationLink)
        {
            TemplateId = EmailConstants.VerifyContactEmailEmployerDemandTemplateId;
            Subject  = "Confirm your contact email address";
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