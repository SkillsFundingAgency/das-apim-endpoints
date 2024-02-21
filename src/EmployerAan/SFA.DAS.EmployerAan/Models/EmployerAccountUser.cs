namespace SFA.DAS.EmployerAan.Models;

public class EmployerAccountUser
{
    public string DasAccountName { get; set; } = null!;
    public string EncodedAccountId { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public bool IsSuspended { get; set; }
    public string DisplayName { get; set; } = null!;
}