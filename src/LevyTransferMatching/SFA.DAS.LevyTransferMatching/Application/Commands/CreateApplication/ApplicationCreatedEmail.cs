using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;

public class ApplicationCreatedEmail : EmailTemplateArguments
{
    public ApplicationCreatedEmail(
        string recipientEmail,
        string employer,
        string reference,
        string unsubscribeUrl,
        string templateId)
    {
        TemplateId = templateId;
        RecipientAddress = recipientEmail;
        Tokens = new Dictionary<string, string>
        {
            {"employer", employer },
            {"reference", reference },
            {"unsubscribe_url", unsubscribeUrl }
        };
    }
}