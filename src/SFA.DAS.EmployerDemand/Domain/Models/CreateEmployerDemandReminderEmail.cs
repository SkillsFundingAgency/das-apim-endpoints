using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.EmployerDemand.Domain.Models
{
    public class CreateEmployerDemandReminderEmail : EmailTemplateArguments
    {
        public CreateEmployerDemandReminderEmail (string recipientEmail, string employerName, string standardName, int standardLevel, string location, int numberOfApprentices)
        {
            TemplateId = EmailConstants.EmployerDemandReminderTemplateId;
            Subject = "We’re still sharing your interest in apprenticeship training with training providers";
            RecipientAddress = recipientEmail;
            Tokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", employerName },
                {"AEDApprenticeshipTrainingCourse", $"{standardName} (level {standardLevel})" },
                {"AEDApprenticeshipLocation", location },
                {"AEDNumberOfApprentices", numberOfApprentices > 0 ? numberOfApprentices.ToString() : "Not sure" },
                {"AEDStopSharingURL",""}
            };
        }
    }
}