using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.Models
{
    public class SendFeedbackEmailsRequest
    {
        public List<NotificationTemplateRequest> NotificationTemplates { get; set; }
        public string EmployerAccountsBaseUrl { get; set; }
        public string EmployerFeedbackBaseUrl { get; set; }
    }

    public class NotificationTemplateRequest
    {
        public string TemplateName { get; set; }
        public Guid TemplateId { get; set; }
    }
}