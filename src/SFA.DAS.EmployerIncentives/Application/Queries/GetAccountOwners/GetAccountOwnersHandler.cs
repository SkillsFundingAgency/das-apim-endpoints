using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetAccountOwners
{
    public class GetAccountOwnersHandler : IRequestHandler<GetAccountOwnersQuery, GetAccountOwnersResult>
    {
        private readonly IAccountsService _accountsService;

        public GetAccountOwnersHandler(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        public async Task<GetAccountOwnersResult> Handle(GetAccountOwnersQuery request, CancellationToken cancellationToken)
        {
            var users = await _accountsService.GetAccountUsers(request.HashedAccountId);

            return new GetAccountOwnersResult
            {
                UserDetails = users.Where(u => u.Role == "Owner")
            };
        }
    }
}