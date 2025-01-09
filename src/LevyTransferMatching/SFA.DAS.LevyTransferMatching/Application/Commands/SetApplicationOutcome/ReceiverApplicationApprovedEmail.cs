using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationOutcome;

public class ReceiverApplicationApprovedEmail : EmailTemplateArguments
{
    public ReceiverApplicationApprovedEmail(
        string recipientEmail,
        string employerName,
        string reference,
        string baseUrl,
        string encodedAccountId,
        string unsubscribeUrl
        )
    {
        TemplateId = "ReceiverApplicationApproved_dev";
        RecipientAddress = recipientEmail;
        Tokens = new Dictionary<string, string>
        {
            {"employer", employerName },
            {"reference", reference },
            {"base_url", baseUrl },
            {"encodedAccountId", encodedAccountId },
            {"unsubscribe_url", unsubscribeUrl }
        };
    }
}