namespace SFA.DAS.SharedOuterApi.Types.Models.Messages
{
    public abstract class EmailTemplateArguments
    {
        public string TemplateId { get; set; }
        public string RecipientAddress { get; set; }
        public string ReplyToAddress { get; set; }
        public Dictionary<string, string> Tokens { get; set; }
    }
}