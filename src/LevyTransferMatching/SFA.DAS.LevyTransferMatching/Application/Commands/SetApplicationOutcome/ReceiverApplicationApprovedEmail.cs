using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationOutcome
{
    public class ReceiverApplicationApprovedEmail : EmailTemplateArguments
    {
        public ReceiverApplicationApprovedEmail(
             string recipientEmail,
            string userName,
            string reference
            )
        {
            TemplateId = "ReceiverApplicationApproved";
            RecipientAddress = recipientEmail;
            Tokens = new Dictionary<string, string>
            {
                {"user_name", userName },
                {"reference", reference }
            };
        }
    }
}