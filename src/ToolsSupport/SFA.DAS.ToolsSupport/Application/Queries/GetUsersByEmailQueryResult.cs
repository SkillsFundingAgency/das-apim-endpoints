using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries;

public class GetUsersByEmailQueryResult
{
    public List<UserProfile> Users { get; set; } = new List<UserProfile>();
}