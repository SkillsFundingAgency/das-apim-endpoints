using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.AccountUsers;

public class GetAccountsQueryResult
{
    public IEnumerable<AccountUser> UserAccountResponse { get; set; }
    public string UserId { get; set; }
    public bool IsSuspended { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}