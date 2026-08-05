using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerAccounts;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class ChangeUserRoleRequest(ChangeUserRoleRequestData data) : IPostApiRequest
{
    public string PostUrl => $"api/support/change-role";
    public object Data { get; set; } = data;
}

public class ChangeUserRoleRequestData
{
    public string Email { get; set; } = "";
    public string HashedAccountId { get; set; } = "";
    public Role Role { get; set; }
}
