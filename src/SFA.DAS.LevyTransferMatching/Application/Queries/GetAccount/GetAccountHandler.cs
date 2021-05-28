using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount
{
    public class GetAccountHandler : IRequestHandler<GetAccountQuery, GetAccountResult>
    {
        private readonly IAccountsService _accountsService;

        public GetAccountHandler(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        public async Task<GetAccountResult> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountsService.GetAccount(request.EncodedAccountId);

            return new GetAccountResult()
            {
                Account = account,
            };
        }
    }
}