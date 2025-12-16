using MediatR;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionUsers
{
    public class GetFeedbackTransactionUsersQuery : IRequest<GetFeedbackTransactionUsersResult>
    {
        public GetFeedbackTransactionUsersQuery(long feedbackTransactionId)
        {
            FeedbackTransactionId = feedbackTransactionId;
        }

        public long FeedbackTransactionId { get; }
    }
}