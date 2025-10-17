using System;

namespace SFA.DAS.EmployerFeedback.InnerApi.Responses
{
    public class GetFeedbackTransactionResponse
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public string AccountName { get; set; }
        public string TemplateName { get; set; }
        public DateTime SendAfter { get; set; }
        public DateTime? SentDate { get; set; }
    }
}