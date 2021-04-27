using System.Collections.Generic;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetAccountOwners
{
    public class GetAccountOwnersResult
    {
        public IEnumerable<UserDetails> UserDetails { get ; set ; }
    }
}