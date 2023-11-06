using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Application.Commands.MetricData
{
    public class CreateMetricDataCommand : IRequest<Unit>
    {
        public MetricDataList MetricDataList { get; set; }
    }
}