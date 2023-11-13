
namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class CreateMetricDataPostRequest
    {
        public IEnumerable<MetricRequestModel> MetricsData { get; set; }
    }
}
