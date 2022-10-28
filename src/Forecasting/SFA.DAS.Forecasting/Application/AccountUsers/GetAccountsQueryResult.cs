using System.Collections.Generic;

namespace SFA.DAS.Forecasting.Application.AccountUsers
{
    public class GetAccountsQueryResult
    {
        public IEnumerable<AccountUser> UserAccountResponse { get; set; }
    }
}