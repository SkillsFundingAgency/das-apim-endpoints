using MediatR;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SendEmails
{
    public class SendEmailsCommand : IRequest<Unit>
    {
        public List<EmailData> EmailDataList { get; set; }

        public class EmailData
        {
            public EmailData(string templateName, string recipientEmailAddress, Dictionary<string, string> tokens)
            {
                TemplateName = templateName;
                RecipientEmailAddress = recipientEmailAddress;
                Tokens = tokens;
            }

            public string TemplateName { get; set; }
            public string RecipientEmailAddress { get; set; }
            public Dictionary<string, string> Tokens { get; set; }
        }
    }
}
