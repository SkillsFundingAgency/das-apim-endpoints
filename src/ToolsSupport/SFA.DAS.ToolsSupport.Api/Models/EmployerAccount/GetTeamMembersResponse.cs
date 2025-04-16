using SFA.DAS.ToolsSupport.Application.Queries.GetTeamMembers;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;

public class GetTeamMembersResponse
{
    public IEnumerable<TeamMember> TeamMembers { get; set; } = [];

    public static explicit operator GetTeamMembersResponse(GetTeamMembersQueryResult source)
    {
        if (source == null) return new GetTeamMembersResponse { TeamMembers = [] };

        return new GetTeamMembersResponse
        {
            TeamMembers = source.TeamMembers
        };
    }
}
