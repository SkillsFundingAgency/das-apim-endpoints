using MediatR;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessEmailTransaction
{
    public class ProcessEmailTransactionCommand : IRequest<ProcessEmailTransactionResponse>
    {
        public long FeedbackTransactionId { get; set; }
        public string ApprenticeName { get; set; }
        public string ApprenticeEmailAddress { get; set; }
        public bool IsFeedbackEmailContactAllowed { get; set; }
        public bool IsEngagementEmailContactAllowed { get; set; }
    }
}
