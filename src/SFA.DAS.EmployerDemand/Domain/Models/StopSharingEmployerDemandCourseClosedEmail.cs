using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.EmployerDemand.Domain.Models
{
    public class StopSharingEmployerDemandCourseClosedEmail : EmailTemplateArguments
    {
        public StopSharingEmployerDemandCourseClosedEmail(
            string recipientEmail, 
            string employerName, 
            string standardName, 
            int standardLevel, 
            string location,
            int numberOfApprentices)
        {
            TemplateId = EmailConstants.StopSharingEmployerDemandCourseClosedTemplateId;
            RecipientAddress = recipientEmail;
            ReplyToAddress = EmailConstants.ReplyToAddress;
            Tokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", employerName },
                {"AEDApprenticeshipTrainingCourse", $"{standardName} (level {standardLevel})" },
                {"AEDApprenticeshipLocation", location },
                {"AEDNumberOfApprentices", numberOfApprentices > 0 ? numberOfApprentices.ToString() : "Not sure" }
            };
        }
    }
}