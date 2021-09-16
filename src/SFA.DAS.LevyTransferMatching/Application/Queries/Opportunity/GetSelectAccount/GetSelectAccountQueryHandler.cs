using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSelectAccount
{
    public class GetSelectAccountQueryHandler : IRequestHandler<GetSelectAccountQuery, GetSelectAccountQueryResult>
    {
        private readonly IUserService _userService;

        public GetSelectAccountQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetSelectAccountQueryResult> Handle(GetSelectAccountQuery request, CancellationToken cancellationToken)
        {
            var userAccounts = await _userService.GetUserAccounts(request.UserId);

            return new GetSelectAccountQueryResult()
            {
                Accounts = userAccounts.Select(x => new GetSelectAccountQueryResult.Account()
                {
                    EncodedAccountId = x.EncodedAccountId,
                    Name = x.DasAccountName,
                }),
            };
        }
    }
}