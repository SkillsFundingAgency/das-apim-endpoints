using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses
{
    public class GetExpiringFundsResponse
    {
        public long AccountId { get; set; }
        public DateTime ProjectionGenerationDate { get; set; }
        public List<GetExpiringFundsListItem> ExpiryAmounts { get; set; }
    }
}
