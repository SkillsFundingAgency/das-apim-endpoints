using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.SharedOuterApi.Controllers.Application.Queries.EmployerAccounts
{
    public class GetUserAccountsQueryHandler : IRequestHandler<GetUserAccountsQuery, GetUserAccountsQueryResult>
    {
        private readonly IEmployerAccountsService _employerAccountService;

        public GetUserAccountsQueryHandler(IEmployerAccountsService employerAccountService)
        {
            _employerAccountService = employerAccountService;
        }
        public async Task<GetUserAccountsQueryResult> Handle(GetUserAccountsQuery request, CancellationToken cancellationToken)
        {
            var employerAccounts = (await _employerAccountService.GetEmployerAccounts(new EmployerProfile
            {
                Email = request.Email,
                UserId = request.UserId
            })).ToList();

            return new GetUserAccountsQueryResult
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