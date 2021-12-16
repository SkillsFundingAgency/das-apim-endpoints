using System.Collections.Generic;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.CreateUser;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.ApimDeveloper.Models
{
    public class VerifyThirdyPartyAccountEmail : EmailTemplateArguments
    {
        public VerifyThirdyPartyAccountEmail(CreateUserCommand command, ApimDeveloperMessagingConfiguration config)
        {
            TemplateId = config.VerifyThirdPartyAccountTemplateId;
            RecipientAddress = command.Email;
            ReplyToAddress = EmailConstants.ReplyToAddress;
            Tokens = new Dictionary<string, string>
            {
                {"Contact", $"{command.FirstName} {command.LastName}" },
                {"ConfirmEmailURL", command.ConfirmationEmailLink }
            };
        }
    }
}