using System;

namespace SFA.DAS.EmployerFeedback.Models
{
    public class UpdateFeedbackTransactionRequest
    {
        public Guid TemplateId { get; set; }
        public int SentCount { get; set; }
        public DateTime SentDate { get; set; }
    }
}
