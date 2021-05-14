using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.EmployerDemand.Domain.Models
{
    public class CreateVerifyEmployerDemandEmail : EmailTemplateArguments
    {
        public CreateVerifyEmployerDemandEmail(string employerName, string trainingCourse, string confirmationLink)
        {
            TemplateId = EmailConstants.VerifyContactEmailEmployerDemandTemplateId;
            Subject  = "Confirm your contact email address";
            Tokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", employerName },
                {"AEDApprenticeshipTrainingCourse", trainingCourse },
                {"AEDConfirmEmailURL", confirmationLink }
            };
        }
    }
}