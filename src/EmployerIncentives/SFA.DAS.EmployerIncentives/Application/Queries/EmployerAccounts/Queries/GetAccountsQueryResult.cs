using System.Collections.Generic;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Application.Queries.EmployerAccounts.Queries
{
    public class GetAccountsQueryResult
    {
        public IEnumerable<AccountUser> UserAccountResponse { get ; set ; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployerUserId { get; set; }
    }
}