namespace SFA.DAS.EmployerPR.Application.UserAccounts.Queries.GetUserAccounts;

public class GetUserAccountsQueryResult
{
    public IEnumerable<AccountUser> UserAccountResponse { get; set; } = Enumerable.Empty<AccountUser>();
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string EmployerUserId { get; set; } = null!;
    public bool IsSuspended { get; set; }
}

