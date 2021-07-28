using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges
{
    public class GetPledgesQueryHandler : IRequestHandler<GetPledgesQuery, GetPledgesResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetPledgesQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetPledgesResult> Handle(GetPledgesQuery request, CancellationToken cancellationToken)
        {
            var pledges = await _levyTransferMatchingService.GetPledges(new GetPledgesRequest());

            return new GetPledgesResult(pledges.Items);
        }
    }
}