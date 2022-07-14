using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Functions
{
    public class SendEmailsRequest
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
