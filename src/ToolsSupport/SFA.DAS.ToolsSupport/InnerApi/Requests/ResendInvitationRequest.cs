using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class ResendInvitationRequest(ResendInvitationRequestData data) : IPostApiRequest
{
    public string PostUrl => $"api/support/resend-invitation";
    public object Data { get; set; } = data;
}

public class ResendInvitationRequestData
{
    public string Email { get; set; } = "";
    public string HashedAccountId { get; set; } = "";
}