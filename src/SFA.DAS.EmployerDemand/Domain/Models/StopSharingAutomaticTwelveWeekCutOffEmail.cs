using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.EmployerDemand.Domain.Models
{
    public class StopSharingAutomaticTwelveWeekCutOffEmail : EmailTemplateArguments
    {
        public StopSharingAutomaticTwelveWeekCutOffEmail (     string recipientEmail, string employerName,
            string standardName, int standardLevel, string location, int numberOfApprentices, string startSharingUrl)
        {
            TemplateId = EmailConstants.StopSharingAutomaticTwelveWeekCutOffTemplateId;
            Subject = "We’ve stopped sharing your interest in apprenticeship training with training providers";
            RecipientAddress = recipientEmail;
            Tokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", employerName },
                {"AEDApprenticeshipTrainingCourse", $"{standardName} (level {standardLevel})" },
                {"AEDApprenticeshipLocation", location },
                {"AEDNumberOfApprentices", numberOfApprentices > 0 ? numberOfApprentices.ToString() : "Not sure" },
                {"AEDStartSharingURL", startSharingUrl}
            };
        }
    }
}