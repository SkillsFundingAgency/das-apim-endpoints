using SFA.DAS.LevyTransferMatching.Models;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetUserAccounts
{
    public class GetUserAccountsResult
    {
        public IEnumerable<UserAccount> UserAccounts { get; set; }
    }
}