using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetAmount
{
    public class GetAmountQueryHandler : IRequestHandler<GetAmountQuery, GetAmountQueryResult>
    {
        private readonly IAccountsService _accountsService;
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetAmountQueryHandler(IAccountsService accountsService, ILevyTransferMatchingService levyTransferMatchingService)
        {
            _accountsService = accountsService;
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetAmountQueryResult> Handle(GetAmountQuery request, CancellationToken cancellationToken)
        {
            // Get the current amount committed to existing pledges first
            var response = await _levyTransferMatchingService.GetPledges(new GetPledgesRequest(request.AccountId));

            GetPledgesQueryResult getPledgesQueryResult = new GetPledgesQueryResult
            {
                Pledges = response?.Pledges.Select(x => new GetPledgesQueryResult.Pledge
                {
                    Amount = x.Amount
                })
            };

            int currentTransferAmount = getPledgesQueryResult?.Pledges?.Count() > 0 ? getPledgesQueryResult.Pledges.Sum(p => p.Amount) : 0;

            // Now get the account and get the remaining transfer allowance after subtracting the above already-committed amount
            var account = await _accountsService.GetAccount(request.AccountId);

            return new GetAmountQueryResult
            {
                RemainingTransferAllowance = account.RemainingTransferAllowance - currentTransferAmount,
                DasAccountName = account.DasAccountName
            };
        }
    }
}