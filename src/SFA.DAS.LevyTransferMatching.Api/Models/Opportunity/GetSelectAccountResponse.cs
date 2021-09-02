using System.Collections.Generic;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSelectAccount;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Opportunity
{
    public class GetSelectAccountResponse
    {
        public IEnumerable<Account> Accounts { get; set; }

        public class Account
        {
            public string EncodedAccountId { get; set; }
            public string Name { get; set; }

            public static implicit operator Account(GetSelectAccountQueryResult.Account account)
            {
                return new Account()
                {
                    EncodedAccountId = account.EncodedAccountId,
                    Name = account.Name,
                };
            }
        }

        public static implicit operator GetSelectAccountResponse(GetSelectAccountQueryResult result)
        {
            return new GetSelectAccountResponse()
            {
                Accounts = result.Accounts.Select(x => (Account)x),
            };
        }
    }
}