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
            var pledges = _levyTransferMatchingService.GetPledges(new GetPledgesRequest(request.AccountId));

            var account = _accountsService.GetAccount(request.AccountId);

            await Task.WhenAll(pledges, account);

            if (pledges == null || pledges.Result == null)
                return null;

            var getPledgesQueryResult = new GetPledgesQueryResult
            {               
                Pledges = pledges.Result.Pledges.Select(x => new GetPledgesQueryResult.Pledge
                {
                    Amount = x.Amount
                })
            };

            int currentTransferAmount = getPledgesQueryResult.Pledges.Sum(p => p.Amount);

            return new GetAmountQueryResult
            {
                RemainingTransferAllowance = account.Result.RemainingTransferAllowance - currentTransferAmount,
                DasAccountName = account.Result.DasAccountName
            };
        }
    }
}