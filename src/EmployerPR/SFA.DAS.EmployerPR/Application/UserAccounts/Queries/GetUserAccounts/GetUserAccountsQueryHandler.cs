using MediatR;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.EmployerPR.Application.UserAccounts.Queries.GetUserAccounts;

public class GetUserAccountsQueryHandler(IEmployerAccountsService _employerAccountsService) : IRequestHandler<GetUserAccountsQuery, GetUserAccountsQueryResult>
{
    public async Task<GetUserAccountsQueryResult> Handle(GetUserAccountsQuery request, CancellationToken cancellationToken)
    {
        var employerAccounts = await _employerAccountsService.GetEmployerAccounts(new EmployerProfile
        {
            Email = request.Email,
            UserId = request.UserId
        });

        /// At the time of writing this, the service returns at least one record even if no accounts were found
        EmployerAccountUser userAccount = employerAccounts.First();

        return new GetUserAccountsQueryResult
        {
            EmployerUserId = userAccount.UserId,
            FirstName = userAccount.FirstName,
            LastName = userAccount.LastName,
            IsSuspended = userAccount.IsSuspended,
            UserAccountResponse = employerAccounts.Where(c => c.EncodedAccountId != null).Select(c => new AccountUser
            {
                DasAccountName = c.DasAccountName,
                EncodedAccountId = c.EncodedAccountId,
                Role = c.Role
            })
        };
    }
}
