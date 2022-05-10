using System;
using System.Collections.Generic;

namespace SFA.DAS.ManageApprenticeships.InnerApi.Responses
{
    public class GetExpiringFundsResponse
    {
        public long AccountId { get; set; }
        public DateTime ProjectionGenerationDate { get; set; }
        public List<GetExpiringFundsListItem> ExpiryAmounts { get; set; }
    }
}
