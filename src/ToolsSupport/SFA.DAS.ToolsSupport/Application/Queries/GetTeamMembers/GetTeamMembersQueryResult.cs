using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetTeamMembers;

public class GetTeamMembersQueryResult
{
    public IEnumerable<TeamMember> TeamMembers { get; set; } = [];
}
