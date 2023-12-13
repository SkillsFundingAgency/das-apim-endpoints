using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class ProcessEmailTransactionRequest : IPostApiRequest
    {
        private string transactionId => ((ProcessEmailTransactionData)Data).FeedbackTransactionId.ToString();
        public string PostUrl => $"api/feedbacktransaction/{transactionId}";

        public object Data { get; set; }

        public ProcessEmailTransactionRequest(ProcessEmailTransactionData data)
        {
            Data = data;
        }
    }

    public class ProcessEmailTransactionData
    {
        public long FeedbackTransactionId { get; set; }
        public string ApprenticeName { get; set; }
        public string ApprenticeEmailAddress { get; set; }
        public bool IsFeedbackEmailContactAllowed { get; set; }
        public bool IsEngagementEmailContactAllowed { get; set; }

        public ProcessEmailTransactionData(long feedbackTransactionId, string apprenticeName, string apprenticeEmailAddress, bool isFeedbackEmailContactAllowed, bool isEngagementEmailContactAllowed)
        {
            FeedbackTransactionId = feedbackTransactionId;
            ApprenticeName = apprenticeName;
            ApprenticeEmailAddress = apprenticeEmailAddress;
            IsFeedbackEmailContactAllowed = isFeedbackEmailContactAllowed;
            IsEngagementEmailContactAllowed = isEngagementEmailContactAllowed;
        }
    }
}
