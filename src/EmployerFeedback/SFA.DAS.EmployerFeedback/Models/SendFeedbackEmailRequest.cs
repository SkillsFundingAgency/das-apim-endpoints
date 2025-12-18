using System;

namespace SFA.DAS.EmployerFeedback.Models
{
    public class SendFeedbackEmailRequest
    {
        public Guid TemplateId { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string EmployerName { get; set; }
        public string AccountHashedId { get; set; }
        public string AccountsBaseUrl { get; set; }
        public string FeedbackBaseUrl { get; set; }
    }
}