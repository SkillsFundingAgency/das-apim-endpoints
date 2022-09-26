
using static SFA.DAS.ApprenticeFeedback.Models.Enums;


namespace SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessEmailTransaction

{
    public class ProcessEmailTransactionResult

    {
        public long FeedbackTransactionId { get; set; }
        public EmailStatus EmailStatus { get; set; }
    }
}
