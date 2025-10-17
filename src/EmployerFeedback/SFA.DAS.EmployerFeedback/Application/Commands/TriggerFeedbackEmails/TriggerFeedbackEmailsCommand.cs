using MediatR;
using SFA.DAS.EmployerFeedback.Models;

namespace SFA.DAS.EmployerFeedback.Application.Commands.TriggerFeedbackEmails
{
    public class TriggerFeedbackEmailsCommand : IRequest
    {
        public TriggerFeedbackEmailsCommand(long feedbackTransactionId, TriggerFeedbackEmailsRequest request)
        {
            FeedbackTransactionId = feedbackTransactionId;
            Request = request;
        }

        public long FeedbackTransactionId { get; }
        public TriggerFeedbackEmailsRequest Request { get; }
    }
}