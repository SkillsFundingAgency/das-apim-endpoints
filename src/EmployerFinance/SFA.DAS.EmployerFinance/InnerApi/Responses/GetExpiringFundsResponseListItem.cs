using System;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses
{
    public class GetExpiringFundsListItem
    {
        public decimal Amount { get; set; }
        public DateTime PayrollDate { get; set; }
    }
}
