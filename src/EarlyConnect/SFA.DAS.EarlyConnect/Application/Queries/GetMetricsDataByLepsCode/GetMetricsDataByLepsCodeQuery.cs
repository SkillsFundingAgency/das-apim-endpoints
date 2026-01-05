using MediatR;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetMetricsDataByLepsCode
{
    public class GetMetricsDataByLepsCodeQuery : IRequest<GetMetricsDataByLepsCodeResult>
    {
        public string LepsCode { get; set; }
    }
}