using SFA.DAS.EarlyConnect.InnerApi.Responses;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetMetricsDataByLepsCode
{
    public class GetMetricsDataByLepsCodeResult
    {
        public ICollection<ApprenticeshipMetricsData>? ListOfMetricsData { get; set; }
    }
}