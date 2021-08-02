using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges
{
    public class GetPledgesQueryHandler : IRequestHandler<GetPledgesQuery, GetPledgesQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetPledgesQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetPledgesQueryResult> Handle(GetPledgesQuery request, CancellationToken cancellationToken)
        {
            var response = await _levyTransferMatchingService.GetPledges(new GetPledgesRequest(request.AccountId));

            return new GetPledgesQueryResult
            {
                Pledges = response?.Items?.Select(x => new GetPledgesQueryResult.Pledge
                {
                    Id = x.Id.Value,
                    Amount = x.Amount,
                    RemainingAmount = x.RemainingAmount,
                    ApplicationCount = x.ApplicationCount
                })
            };
        }
    }
}
