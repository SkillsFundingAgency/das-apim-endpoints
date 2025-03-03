using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Reservations.Application.Accounts.Queries;

public record GetAccountUsersQuery(long AccountId) : IRequest<GetAccountUsersQueryResult>;

public record GetAccountUsersQueryResult
{
    public IEnumerable<TeamMember> TeamMembers { get; init; }
}

public class GetAccountUsersQueryHandler(IEmployerAccountsService employerAccountsService) : IRequestHandler<GetAccountUsersQuery, GetAccountUsersQueryResult>
{
    public async Task<GetAccountUsersQueryResult> Handle(GetAccountUsersQuery request, CancellationToken cancellationToken)
    {
        var teamMembers = await employerAccountsService.GetTeamMembers(request.AccountId);

        return new GetAccountUsersQueryResult
        {
            TeamMembers = teamMembers.Select(user => new TeamMember
            {
                Name = user.Name,
                Status = user.Status,
                Email = user.Email,
                Role = user.Role,
                UserRef = user.UserRef,
                CanReceiveNotifications = user.CanReceiveNotifications
            })
        };
    }
}