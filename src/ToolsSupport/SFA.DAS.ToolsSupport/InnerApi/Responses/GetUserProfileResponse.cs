namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class GetUserProfileResponse
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string GovUkIdentifier { get; set; } = "";
    public bool IsSuspended { get; set; }
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }
}