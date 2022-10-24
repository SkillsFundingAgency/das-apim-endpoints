using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApimDeveloper.Models;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.ApimDeveloper.Application.EmployerAccounts.Queries
{
    public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, GetAccountsQueryResult>
    {
        private readonly IEmployerAccountsService _employerAccountService;

        public GetAccountsQueryHandler (IEmployerAccountsService employerAccountService)
        {
            _employerAccountService = employerAccountService;
        }
        public async Task<GetAccountsQueryResult> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var returnList = await _employerAccountService.GetEmployerAccounts(request.UserId, request.Email);
            
            return new GetAccountsQueryResult
            {
                UserAccountResponse = returnList.Select(c=> new AccountUser
                {
                    DasAccountName = c.DasAccountName,
                    EncodedAccountId = c.EncodedAccountId,
                    Role = c.Role
                })
            };
        }
    }
}