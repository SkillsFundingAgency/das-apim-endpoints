using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.Models
{
    public class SendEmailCommand
    {
        public string TemplateId { get; set; }
        public string RecipientAddress { get; set; }
        public string ReplyToAddress { get; set; }
        public string Subject { get; set; }
        public Dictionary<string, string> Tokens { get; set; }
    }
}