using System;
using System.Collections.Generic;
using SFA.DAS.ManageApprenticeships.InnerApi.Responses;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.GetAccountProjectionSummary
{
    public class GetAccountProjectionSummaryQueryResult
    {
        public long AccountId { get; set; }
        public DateTime? ProjectionGenerationDate { get; set; }
        public int NumberOfMonths { get; set; }
        public decimal FundsIn { get; set; }
        public decimal FundsOut { get; set; }
        public IEnumerable<GetExpiringFundsListItem> ExpiryAmounts { get; set; }
    }
}
