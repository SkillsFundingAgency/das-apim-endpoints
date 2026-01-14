using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class UpsertAccountsRequest : IPostApiRequest<AccountsData>
    {
        public UpsertAccountsRequest(AccountsData accounts)
        {
            Data = accounts;
        }
        public string PostUrl => "api/accounts";
        public AccountsData Data { get; set; }
    }
    public class AccountsData
    {
        public List<UpsertAccountsData> Accounts { get; set; }
    }
    public class UpsertAccountsData
    {
        public long AccountId { get; set; }
        public string AccountName { get; set; }
    }
}
