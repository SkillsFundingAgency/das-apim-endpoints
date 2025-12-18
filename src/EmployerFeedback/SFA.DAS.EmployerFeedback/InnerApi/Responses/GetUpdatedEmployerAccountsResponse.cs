using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.InnerApi.Responses
{
    public class GetUpdatedEmployerAccountsResponse
    {
        public List<UpdatedEmployerAccounts> Data { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }

    public class UpdatedEmployerAccounts
    {
        public long AccountId { get; set; }
        public string AccountName { get; set; }
    }
}
