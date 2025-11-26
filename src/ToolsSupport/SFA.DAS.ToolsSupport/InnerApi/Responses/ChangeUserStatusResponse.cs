namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class ChangeUserStatusResponse
{
    public string? Id { get; set; }
    public IDictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
}

