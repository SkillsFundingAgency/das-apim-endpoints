using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.AccountUsers;

public class GetAccountsQueryResult
{
    public IEnumerable<AccountUser> UserAccountResponse { get; set; }
}