using System.Collections.Generic;
using SFA.DAS.ApimDeveloper.Models;

namespace SFA.DAS.ApimDeveloper.Application.EmployerAccounts.Queries
{
    public class GetAccountsQueryResult
    {
        public IEnumerable<AccountUser> UserAccountResponse { get ; set ; }
        public string EmployerUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSuspended { get; set; }
    }
}