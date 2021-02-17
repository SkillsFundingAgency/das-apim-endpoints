using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts
{
    public class AccountLegalEntity
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long LegalEntityId { get; set; }
        public string Name { get; set; }
    }
}
