using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetTeamMembers;

public class GetTeamMembersQueryHander(
    IAccountsService accountsService,
    ILogger<GetTeamMembersQueryHander> logger)
        : IRequestHandler<GetTeamMembersQuery, GetTeamMembersQueryResult>
{
    public async Task<GetTeamMembersQueryResult> Handle(GetTeamMembersQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Team Members for Account {account}", query.AccountId);

        var teamMembers = await accountsService.GetAccountTeamMembers(query.AccountId);

        return new GetTeamMembersQueryResult
        {
            TeamMembers = teamMembers ?? []
        };
    }
}