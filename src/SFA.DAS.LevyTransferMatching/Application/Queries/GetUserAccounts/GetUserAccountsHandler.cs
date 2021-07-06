using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetUserAccounts
{
    public class GetUserAccountsHandler : IRequestHandler<GetUserAccountsQuery, GetUserAccountsResult>
    {
        private readonly IUserService _userService;

        public GetUserAccountsHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetUserAccountsResult> Handle(GetUserAccountsQuery request, CancellationToken cancellationToken)
        {
            var userAccounts = await _userService.GetUserAccounts(request.UserId);

            return new GetUserAccountsResult()
            {
                UserAccounts = userAccounts,
            };
        }
    }
}