using System.Collections.Generic;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetOpportunityApply;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Opportunity
{
    public class GetOpportunityApplyResponse
    {
        public IEnumerable<Account> Accounts { get; set; }

        public class Account
        {
            public string EncodedAccountId { get; set; }
            public string Name { get; set; }

            public static implicit operator Account(GetOpportunityApplyQueryResult.Account account)
            {
                return new Account()
                {
                    EncodedAccountId = account.EncodedAccountId,
                    Name = account.Name,
                };
            }
        }

        public static implicit operator GetOpportunityApplyResponse(GetOpportunityApplyQueryResult result)
        {
            return new GetOpportunityApplyResponse()
            {
                Accounts = result.Accounts.Select(x => (Account)x),
            };
        }
    }
}