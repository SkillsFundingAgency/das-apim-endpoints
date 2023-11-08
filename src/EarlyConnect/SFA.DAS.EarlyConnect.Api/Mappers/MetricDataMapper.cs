using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Api.Mappers
{
    public static class MetricDataMapper
    {
        public static MetricDataList MapFromMetricDataPostRequest(this CreateMetricDataPostRequest request)
        {
            var metricDataList = new List<MetricData>();

            foreach (MetricRequestModel model in request.MetricData)
            {
                var metricData = new MetricData
                {
                    Region = model.Region,
                    IntendedStartYear = model.IntendedStartYear,
                    MaxTravelInMiles = model.MaxTravelInMiles,
                    WillingnessToRelocate = model.WillingnessToRelocate,
                    NoOfGCSCs = model.NoOfGCSCs,
                    NoOfStudents = model.NoOfStudents,
                    MetricFlagRequestModel = model.MetricFlagRequestModel != null
                        ? new InnerApi.Requests.MetricFlagRequestModel
                        {
                            MetricsFlags = model.MetricFlagRequestModel.MetricFlags
                        }
                        : null

                };

                metricDataList.Add(metricData);
            }

            return new MetricDataList { MetricsData = metricDataList };
        }
    }
}
