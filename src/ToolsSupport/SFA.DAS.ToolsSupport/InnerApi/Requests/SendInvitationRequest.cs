using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class SendInvitationRequest(SendInvitationRequestData data) : IPostApiRequest
{
    public string PostUrl => $"api/support/send-invitation";
    public object Data { get; set; } = data;
}

public class SendInvitationRequestData
{
    public string HashedAccountId { get; set; } = "";
    public string NameOfPersonBeingInvited { get; set; } = "";
    public string EmailOfPersonBeingInvited { get; set; } = "";
    public int RoleOfPersonBeingInvited { get; set; }
}