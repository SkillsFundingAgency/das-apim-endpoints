using SFA.DAS.EarlyConnect.Application.Queries.GetMetricsDataByLepsCode;

namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class GetMetricsDataListByLepsCodeResponse
    {
        public ICollection<GetMetricsDataByLepsCodeResponse>? ListOfMetricsData { get; set; }

        public static implicit operator GetMetricsDataListByLepsCodeResponse(GetMetricsDataByLepsCodeResult source)
        {
            return new GetMetricsDataListByLepsCodeResponse
            {
                ListOfMetricsData = source.ListOfMetricsData.Select(c => (GetMetricsDataByLepsCodeResponse)c).ToList()
            };
        }
    }
}