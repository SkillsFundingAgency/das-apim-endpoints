
using System;
using SFA.DAS.SharedOuterApi.Interfaces;


namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GetFeedbackTransactionsToEmailRequest : IGetApiRequest
    {
        public int BatchSize { get; set; }

        public GetFeedbackTransactionsToEmailRequest(int batchSize)
        {
            BatchSize = batchSize;
        }

        public string GetUrl => $"api/feedbacktransaction/{BatchSize}";
    }
}
