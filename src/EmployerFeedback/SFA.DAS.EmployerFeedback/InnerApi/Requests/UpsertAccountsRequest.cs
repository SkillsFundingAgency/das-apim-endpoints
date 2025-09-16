using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class UpsertAccountsRequest : IPostApiRequest<List<UpsertAccountsData>>
    {
        public UpsertAccountsRequest(List<UpsertAccountsData> accounts)
        {
            Data = accounts;
        }
        public string PostUrl => "api/account";
        public List<UpsertAccountsData> Data { get; set; }
    }
    public class UpsertAccountsData
    {
        public long AccountId { get; set; }
        public string AccountName { get; set; }
    }
}
