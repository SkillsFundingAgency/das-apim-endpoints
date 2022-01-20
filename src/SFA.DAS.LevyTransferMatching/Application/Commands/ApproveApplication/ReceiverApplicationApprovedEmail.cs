using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication
{
    public class ReceiverApplicationApprovedEmail : EmailTemplateArguments
    {
        public ReceiverApplicationApprovedEmail(
            string recipientEmail, 
            string receiverName,
            string senderName,
            string baseUrl,
            string encodedAccountId)
        {
            TemplateId = "11750b40-eb1c-4731-ab9c-d6f692743f37";
            RecipientAddress = recipientEmail;
            Tokens = new Dictionary<string, string>
            {
                {"ReceiverName", receiverName },
                {"SenderName", senderName },
                {"BaseUrl", baseUrl },
                {"EncodedAccountId", encodedAccountId }
            };
        }
    }
}