using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;

public class ChangeUserRoleRequest
{
    public string HashedAccountId { get; set; } = "";
    public string Email { get; set; } = "";
    public required Role Role { get; set; }
}
