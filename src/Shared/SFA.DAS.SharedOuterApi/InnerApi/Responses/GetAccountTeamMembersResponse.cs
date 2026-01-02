namespace SFA.DAS.SharedOuterApi.InnerApi.Responses;

public class GetAccountTeamMembersResponse
{
    public string UserRef { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }
    public bool CanReceiveNotifications { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public int Status { get; set; }
}