using System.Collections.Generic;

namespace SFA.DAS.Reservations.Application.AccountUsers.Queries
{
    public class GetAccountsQueryResult
    {
        public string UserId { get; set; }
        public bool IsSuspended { get; set; }
        public IEnumerable<AccountUser> UserAccountResponse { get ; set ; }
    }
}