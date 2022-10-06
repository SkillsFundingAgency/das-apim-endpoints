using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetPledges
{
    public class GetPledgesQuery : IRequest<GetPledgesQueryResult>
    {
        public long AccountId { get; set; }
    }
}