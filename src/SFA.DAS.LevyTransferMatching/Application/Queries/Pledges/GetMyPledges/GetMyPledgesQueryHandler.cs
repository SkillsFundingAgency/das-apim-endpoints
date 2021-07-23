using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetMyPledges
{
    public class GetMyPledgesQueryHandler : IRequestHandler<GetMyPledgesQuery, GetMyPledgesQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetMyPledgesQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetMyPledgesQueryResult> Handle(GetMyPledgesQuery request, CancellationToken cancellationToken)
        {
            var pledges = await _levyTransferMatchingService.GetPledges();

            return new GetMyPledgesQueryResult
            {
                Pledges = pledges.Select(x => new GetMyPledgesQueryResult.MyPledge { Id = x.Id.Value, Amount = x.Amount })
            };
        }
    }
}
