using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class SuspendEmployerUserRequest(string identifier, ChangeUserStatusRequestData data) : IPostApiRequest
{
    public string PostUrl => $"api/users/{identifier}/suspend";
    public object Data { get; set; } = data;
}

public class ResumeEmployerUserRequest(string identifier, ChangeUserStatusRequestData data) : IPostApiRequest
{
    public string PostUrl => $"api/users/{identifier}/resume";
    public object Data { get; set; } = data;
}

public class ChangeUserStatusRequestData
{
    public required string ChangedByUserId { get; set; }
    public required string ChangedByEmail { get; set; }
}

