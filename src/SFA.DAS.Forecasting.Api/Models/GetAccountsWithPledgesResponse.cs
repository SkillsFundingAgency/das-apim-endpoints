using System.Collections.Generic;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetAccountsWithPledges;

namespace SFA.DAS.Forecasting.Api.Models
{
    public class GetAccountsWithPledgesResponse
    {
        public List<long> AccountIds { get; set; }

        public static implicit operator GetAccountsWithPledgesResponse(GetAccountsWithPledgesQueryResult source)
        {
            return new GetAccountsWithPledgesResponse
            {
                AccountIds = source.AccountIds
            };
        }
    }
}
