﻿using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.EmployerDemand.Domain.Models
{
    public class StopSharingExpiredEmployerDemandEmail : EmailTemplateArguments
    {
        public StopSharingExpiredEmployerDemandEmail (
            string recipientEmail, 
            string employerName, 
            string standardName, 
            int standardLevel, 
            string location,
            int numberOfApprentices,
            string startSharingUrl)
        {
            TemplateId = EmailConstants.StopSharingExpiredEmployerDemandTemplateId;
            RecipientAddress = recipientEmail;
            ReplyToAddress = EmailConstants.ReplyToAddress;
            Tokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", employerName },
                {"AEDApprenticeshipTrainingCourse", $"{standardName} (level {standardLevel})" },
                {"AEDApprenticeshipLocation", location },
                {"AEDNumberOfApprentices", numberOfApprentices > 0 ? numberOfApprentices.ToString() : "Not sure" },
                {"AEDStartSharingURL", startSharingUrl }
            };   
        }
    }
}