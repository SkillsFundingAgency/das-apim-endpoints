using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class GetFeedbackTransactionRequest : IGetApiRequest
    {
        public GetFeedbackTransactionRequest(long feedbackTransactionId)
        {
            FeedbackTransactionId = feedbackTransactionId;
        }

        public long FeedbackTransactionId { get; }

        public string GetUrl => $"api/feedbacktransactions/{FeedbackTransactionId}";
    }
}