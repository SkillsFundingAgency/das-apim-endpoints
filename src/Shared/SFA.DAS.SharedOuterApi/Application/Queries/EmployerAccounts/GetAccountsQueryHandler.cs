using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.SharedOuterApi.Application.Queries.EmployerAccounts
{
    public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, GetAccountsQueryResult>
    {
        private readonly IEmployerAccountsService _employerAccountService;

        public GetAccountsQueryHandler(IEmployerAccountsService employerAccountService)
        {
            _employerAccountService = employerAccountService;
        }
        public async Task<GetAccountsQueryResult> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var employerAccounts = (await _employerAccountService.GetEmployerAccounts(new EmployerProfile
            {
                Email = request.Email,
                UserId = request.UserId
            })).ToList();

            return new GetAccountsQueryResult
            {
                EmployerUserId = employerAccounts.FirstOrDefault()?.UserId,
                FirstName = employerAccounts.FirstOrDefault()?.FirstName,
                LastName = employerAccounts.FirstOrDefault()?.LastName,
                IsSuspended = employerAccounts.FirstOrDefault()?.IsSuspended ?? false,
                UserAccountResponse = employerAccounts.Where(c => c.EncodedAccountId != null).Select(c => new AccountUser
                {
                    DasAccountName = c.DasAccountName,
                    EncodedAccountId = c.EncodedAccountId,
                    Role = c.Role
                })
            };
        }
    }
}