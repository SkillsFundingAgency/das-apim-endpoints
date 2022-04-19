using System;

namespace SFA.DAS.ManageApprenticeships.InnerApi.Responses
{
    public class GetExpiringFundsListItem
    {
        public decimal Amount { get; set; }
        public DateTime PayrollDate { get; set; }
    }
}
