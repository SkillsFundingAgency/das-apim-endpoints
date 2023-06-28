using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetAmount
{
    public class GetAmountQueryHandler : IRequestHandler<GetAmountQuery, GetAmountQueryResult>
    {
        private readonly IAccountsService _accountsService;

        public GetAmountQueryHandler(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        public async Task<GetAmountQueryResult> Handle(GetAmountQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountsService.GetAccount(request.EncodedAccountId);

            return new GetAmountQueryResult
            {
                RemainingTransferAllowance = account.RemainingTransferAllowance
            };
        }
    }
}