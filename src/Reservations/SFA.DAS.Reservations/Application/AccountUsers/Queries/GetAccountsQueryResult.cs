using System.Collections.Generic;

namespace SFA.DAS.Reservations.Application.AccountUsers.Queries
{
    public class GetAccountsQueryResult
    {
        public IEnumerable<AccountUser> UserAccountResponse { get ; set ; }
    }
}