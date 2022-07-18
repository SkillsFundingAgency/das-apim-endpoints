using System.Collections.Generic;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.SendEmailToChangePassword;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.ApimDeveloper.Models
{
    public class ChangePasswordEmail : EmailTemplateArguments
    {
        public ChangePasswordEmail(SendEmailToChangePasswordCommand command, ApimDeveloperMessagingConfiguration config)
        {
            TemplateId = config.ChangePasswordTemplateId;
            RecipientAddress = command.Email;
            ReplyToAddress = EmailConstants.ReplyToAddress;
            Tokens = new Dictionary<string, string>
            {
                {"Contact", $"{command.FirstName} {command.LastName}" },
                {"ChangePasswordURL", command.ChangePasswordUrl }
            };
        }
    }
}