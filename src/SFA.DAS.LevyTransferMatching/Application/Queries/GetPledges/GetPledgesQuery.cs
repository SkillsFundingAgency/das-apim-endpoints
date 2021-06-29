using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges
{
    public class GetPledgesQuery : IRequest<GetPledgesResult>
    {
        public int? PledgeId { get; set; }
    }
}