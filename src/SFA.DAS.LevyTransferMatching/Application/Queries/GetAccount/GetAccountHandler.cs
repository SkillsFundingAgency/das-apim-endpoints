using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount
{
    public class GetAccountHandler : IRequestHandler<GetAccountQuery, GetAccountResult>
    {
        public async Task<GetAccountResult> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            return new GetAccountResult()
            {
                Account = new Models.Account()
                {
                    RemainingTransferAllowance = 123,
                }
            };
        }
    }
}