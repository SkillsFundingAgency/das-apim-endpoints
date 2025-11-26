using MediatR;
using SFA.DAS.EmployerFeedback.Models;

namespace SFA.DAS.EmployerFeedback.Application.Commands.SendFeedbackEmails
{
    public class SendFeedbackEmailsCommand : IRequest
    {
        public SendFeedbackEmailsCommand(long feedbackTransactionId, SendFeedbackEmailsRequest request)
        {
            FeedbackTransactionId = feedbackTransactionId;
            Request = request;
        }

        public long FeedbackTransactionId { get; }
        public SendFeedbackEmailsRequest Request { get; }
    }
}