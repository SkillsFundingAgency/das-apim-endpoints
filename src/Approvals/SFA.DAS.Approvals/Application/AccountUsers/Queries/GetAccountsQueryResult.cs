using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.AccountUsers.Queries
{
    public class GetAccountsQueryResult
    {
        public IEnumerable<AccountUser> UserAccountResponse { get ; set ; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployerUserId { get; set; }
    }
}