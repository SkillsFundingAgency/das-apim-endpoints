namespace SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;

public class SupportCreateInvitationRequest
{
    public string HashedAccountId { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public required int Role { get; set; }
}
