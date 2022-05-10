using MediatR;

namespace SFA.DAS.Forecasting.Application.Pledges.Queries.GetPledges
{
    public class GetPledgesQuery : IRequest<GetPledgesQueryResult>
    {
        public long AccountId { get; set; }
    }
}
