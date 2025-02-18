namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class TeamMember
{
    public string UserRef { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
    public bool CanReceiveNotifications { get; set; }
    public int Status { get; set; }
}
