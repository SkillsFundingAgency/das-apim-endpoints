using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Finance;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses.Finance;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges
{
    public class GetPledgesQueryHandler : IRequestHandler<GetPledgesQuery, GetPledgesQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IFinanceApiClient<FinanceApiConfiguration> _financeApiClient;


        public GetPledgesQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _financeApiClient = financeApiClient;
        }

        public async Task<GetPledgesQueryResult> Handle(GetPledgesQuery request, CancellationToken cancellationToken)
        {
            var ltmTask = _levyTransferMatchingService.GetPledges(new GetPledgesRequest(request.AccountId));

            var fundingTask = _financeApiClient.Get<GetTransferAllowanceResponse>(new GetTransferAllowanceByAccountIdRequest(request.AccountId));

            await Task.WhenAll(ltmTask, fundingTask);

            var ltmResponse = await ltmTask;
            var fundingResponse = await fundingTask;

            return new GetPledgesQueryResult
            {
                RemainingTransferAllowance = fundingResponse.RemainingTransferAllowance.Value,
                Pledges = ltmResponse.Pledges.Select(x => new GetPledgesQueryResult.Pledge
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    RemainingAmount = x.RemainingAmount,
                    ApplicationCount = x.ApplicationCount,
                    Status = x.Status
                })
            };
        }
    }
}
