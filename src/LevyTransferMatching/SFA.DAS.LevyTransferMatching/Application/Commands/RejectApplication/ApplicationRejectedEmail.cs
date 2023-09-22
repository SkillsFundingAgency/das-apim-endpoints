using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication
{
    public class ApplicationRejectedEmail : EmailTemplateArguments
    {
        public ApplicationRejectedEmail(
            string recipientEmail, 
            string userName,
            string reference,
            string baseUrl)
        {
            TemplateId = "ReceiverApplicationRejected";
            RecipientAddress = recipientEmail;
            Tokens = new Dictionary<string, string>
            {
                {"user_name", userName },
                {"reference", reference },
                {"base_url", baseUrl }
            };
        }
    }
}