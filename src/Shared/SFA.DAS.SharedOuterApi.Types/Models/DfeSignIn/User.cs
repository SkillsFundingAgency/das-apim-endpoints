namespace SFA.DAS.SharedOuterApi.Types.Models.DfeSignIn;

public class User
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int UserStatus { get; set; }
}