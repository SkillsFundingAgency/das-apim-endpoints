using System;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses
{
    public class GetProjectionCalculationResponse
    {
        public long AccountId { get; set; }
        public DateTime ProjectionGenerationDate { get; set; }
        public int NumberOfMonths { get; set; }
        public decimal FundsIn { get; set; }
        public decimal FundsOut { get; set; }
    }
}
