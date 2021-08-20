using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetOpportunityApply
{
    public class GetOpportunityApplyQueryResult
    {
        public IEnumerable<Account> Accounts { get; set; }

        public class Account
        {
            public string EncodedAccountId { get; set; }
            public string Name { get; set; }
        }
    }
}