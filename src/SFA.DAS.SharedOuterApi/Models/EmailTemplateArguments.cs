using System.Collections.Generic;
using NServiceBus;

namespace SFA.DAS.SharedOuterApi.Models
{
    public abstract class EmailTemplateArguments : IMessage
    {
        public string TemplateId { get; set; }
        public string RecipientAddress { get; set; }
        public string ReplyToAddress { get; set; }
        public string Subject { get; set; }
        public Dictionary<string, string> Tokens { get; set; }
    }
}