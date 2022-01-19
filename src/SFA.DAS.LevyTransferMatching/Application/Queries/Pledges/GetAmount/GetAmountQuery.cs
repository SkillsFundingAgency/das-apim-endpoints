using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetAmount
{
    public class GetAmountQuery : IRequest<GetAmountQueryResult>
    {
        public long AccountId { get; set; }
    }
}
