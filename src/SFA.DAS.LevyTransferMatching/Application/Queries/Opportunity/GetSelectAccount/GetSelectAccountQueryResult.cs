using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSelectAccount
{
    public class GetSelectAccountQueryResult
    {
        public IEnumerable<Account> Accounts { get; set; }

        public class Account
        {
            public string EncodedAccountId { get; set; }
            public string Name { get; set; }
        }
    }
}