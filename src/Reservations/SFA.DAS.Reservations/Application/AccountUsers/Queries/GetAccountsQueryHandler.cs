using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Reservations.Application.AccountUsers.Queries
{
    public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, GetAccountsQueryResult>
    {
        private readonly IEmployerAccountsService _employerAccountsService;

        public GetAccountsQueryHandler(IEmployerAccountsService employerAccountsService)
        {
            _employerAccountsService = employerAccountsService;
        }
        public async Task<GetAccountsQueryResult> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var employerAccounts = (await _employerAccountsService.GetEmployerAccounts(new EmployerProfile
            {
                Email = request.Email,
                UserId = request.UserId
            })).ToList();
            
            return new GetAccountsQueryResult
            {
                UserId = employerAccounts.FirstOrDefault()?.UserId,
                IsSuspended = employerAccounts.FirstOrDefault()?.IsSuspended ?? false,
                UserAccountResponse = employerAccounts.Where(c=>c.EncodedAccountId != null).Select(c=> new AccountUsers.AccountUser
                {
                    DasAccountName = c.DasAccountName,
                    EncodedAccountId = c.EncodedAccountId,
                    Role = c.Role
                })
            };
        }
    }
}