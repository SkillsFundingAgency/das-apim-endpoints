using System.Collections.Generic;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Models;

namespace SFA.DAS.ApimDeveloper.Application.EmployerAccounts.Queries
{
    public class GetAccountsQueryResult
    {
        public IEnumerable<AccountUser> UserAccountResponse { get ; set ; }
    }
}