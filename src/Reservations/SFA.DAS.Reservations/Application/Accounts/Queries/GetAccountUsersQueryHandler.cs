using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Reservations.Application.Accounts.Queries;

public record GetAccountUsersQuery(long AccountId) : IRequest<GetAccountUsersQueryResult> { }

public record GetAccountUsersQueryResult
{
    public IEnumerable<User> AccountUsersResponse { get; init; }
}

public class GetAccountUsersQueryHandler(IEmployerAccountsService employerAccountsService) : IRequestHandler<GetAccountUsersQuery, GetAccountUsersQueryResult>
{
    public async Task<GetAccountUsersQueryResult> Handle(GetAccountUsersQuery request, CancellationToken cancellationToken)
    {
        var accountUsers = await employerAccountsService.GetAccountUsers(request.AccountId);

        return new GetAccountUsersQueryResult
        {
            AccountUsersResponse = accountUsers.Select(user => new User
            {
                Email = user.Email,
                Role = user.Role,
                UserRef = user.UserRef,
                CanReceiveNotifications = user.CanReceiveNotifications
            })
        };
    }
}