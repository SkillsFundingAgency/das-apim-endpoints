using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerAccounts;

namespace SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;

public class ChangeUserRoleRequest
{
    public string HashedAccountId { get; set; } = "";
    public string Email { get; set; } = "";
    public required Role Role { get; set; }
}
