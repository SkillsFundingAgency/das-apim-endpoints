using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Reservations.Application.Accounts.Queries;

public record GetUsersQuery(long AccountId) : IRequest<GetUsersQueryResult> { }

public record GetUsersQueryResult
{
    public IEnumerable<User> AccountUsersResponse { get; init; }
}

public class GetUsersQueryHandler(IEmployerAccountsService employerAccountsService) : IRequestHandler<GetUsersQuery, GetUsersQueryResult>
{
    public async Task<GetUsersQueryResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var accountUsers = await employerAccountsService.GetAccountUsers(request.AccountId);

        return new GetUsersQueryResult
        {
            AccountUsersResponse = accountUsers.Select(user => new User
            {
                Email = user.Email,
                Name = user.Name,
                Role = user.Role,
                Status = user.Status,
                UserRef = user.UserRef,
                CanReceiveNotifications = user.CanReceiveNotifications
            })
        };
    }
}