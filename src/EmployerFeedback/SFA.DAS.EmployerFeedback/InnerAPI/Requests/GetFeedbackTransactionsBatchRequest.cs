using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class GetFeedbackTransactionsBatchRequest : IGetApiRequest
    {
        public GetFeedbackTransactionsBatchRequest(int batchSize)
        {
            BatchSize = batchSize;
        }

        public int BatchSize { get; }

        public string GetUrl => $"api/feedbacktransactions?batchsize={BatchSize}";
    }
}