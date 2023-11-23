using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateMetricData
{
    public class CreateMetricDataCommand : IRequest<CreateMetricsDataCommandResult>
    {
        public MetricDataList metricsData { get; set; }
    }
}