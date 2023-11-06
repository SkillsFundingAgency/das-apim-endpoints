using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class CreateMetricDataRequest : IPostApiRequest
    {
        public object Data { get; set; }

        public CreateMetricDataRequest(MetricDataList metricDataList)
        {
            Data = metricDataList;
        }

        public string PostUrl => "api/metrics-data";
    }
}