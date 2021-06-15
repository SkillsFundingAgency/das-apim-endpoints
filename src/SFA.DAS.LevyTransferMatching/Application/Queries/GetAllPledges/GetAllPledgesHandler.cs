using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetAllPledges
{
    public class GetAllPledgesHandler : IRequestHandler<GetAllPledgesQuery, GetAllPledgesResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetAllPledgesHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetAllPledgesResult> Handle(GetAllPledgesQuery request, CancellationToken cancellationToken)
        {
            var result = await _levyTransferMatchingService.GetAllPledges();

            return new GetAllPledgesResult()
            {
                Pledges = result
            };
        }
    }
}
