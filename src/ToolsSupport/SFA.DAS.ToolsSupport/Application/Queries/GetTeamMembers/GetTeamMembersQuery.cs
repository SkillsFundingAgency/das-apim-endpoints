using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetTeamMembers;

public class GetTeamMembersQuery : IRequest<GetTeamMembersQueryResult>
{
    public long AccountId { get; set; }
}
