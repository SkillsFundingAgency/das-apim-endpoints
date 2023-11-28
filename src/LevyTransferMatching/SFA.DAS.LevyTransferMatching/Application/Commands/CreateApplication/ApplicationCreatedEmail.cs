using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication
{
    public class ApplicationCreatedEmail : EmailTemplateArguments
    {
        public ApplicationCreatedEmail(
            string recipientEmail, 
            string userName,
            string reference,
            string templateId)
        {
            TemplateId = templateId;
            RecipientAddress = recipientEmail;
            Tokens = new Dictionary<string, string>
            {
                {"user_name", userName },
                {"reference", reference }                
            };
        }
    }
}