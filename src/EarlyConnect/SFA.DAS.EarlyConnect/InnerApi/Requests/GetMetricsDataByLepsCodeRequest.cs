using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class GetMetricsDataByLepsCodeRequest : IGetApiRequest
    {
        public string LepsCode { get; set; }

        public GetMetricsDataByLepsCodeRequest(string lepsCode)
        {
            LepsCode = lepsCode;
        }

        public string GetUrl => $"api/metrics-data/{LepsCode}";
    }
}