using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses
{
    public class GetAccountsResponse
    {
        public List<EmployerAccount> EmployerAccounts { get; set; }

        public class EmployerAccount
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }
    }
}
