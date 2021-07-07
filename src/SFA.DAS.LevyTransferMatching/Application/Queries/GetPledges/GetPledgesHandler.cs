using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAllPledges;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges
{
    public class GetPledgesHandler : IRequestHandler<GetPledgesQuery, GetPledgesResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetPledgesHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetPledgesResult> Handle(GetPledgesQuery request, CancellationToken cancellationToken)
        {
            var result = await _levyTransferMatchingService.GetPledges();
            return new GetPledgesResult(result);
        }
    }
}
