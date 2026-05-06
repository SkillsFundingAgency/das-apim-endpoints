
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;


namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GetFeedbackTransactionsToEmailRequest : IGetApiRequest
    {
        public int BatchSize { get; set; }

        public GetFeedbackTransactionsToEmailRequest(int batchSize)
        {
            BatchSize = batchSize;
        }

        public string GetUrl => $"api/feedbacktransaction?batchSize={BatchSize}";
    }
}
