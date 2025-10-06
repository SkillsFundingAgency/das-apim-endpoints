using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class GetAccountsBatchRequest : IGetApiRequest
    {
        public GetAccountsBatchRequest(int batchSize)
        {
            BatchSize = batchSize;
        }

        public int BatchSize { get; }

        public string GetUrl => $"api/accounts?batchsize={BatchSize}";
    }
}