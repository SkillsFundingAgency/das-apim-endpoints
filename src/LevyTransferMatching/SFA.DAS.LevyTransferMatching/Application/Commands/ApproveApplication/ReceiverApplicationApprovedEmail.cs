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
            TemplateId = "ReceiverApplicationApproved";
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