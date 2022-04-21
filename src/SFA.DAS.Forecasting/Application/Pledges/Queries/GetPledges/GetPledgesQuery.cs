using MediatR;

namespace SFA.DAS.Forecasting.Application.Pledges.Queries.GetPledges
{
    public class GetPledgesQuery : IRequest<GetPledgesQueryResult>
    {
        public int Page { get; set; } 
        public int PageSize { get; set; }
    }
}
