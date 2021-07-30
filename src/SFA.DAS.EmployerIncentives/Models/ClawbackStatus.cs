using System;

namespace SFA.DAS.EmployerIncentives.Models
{
    public class ClawbackStatus
    {
        public decimal ClawbackAmount { get; set; }
        public DateTime? ClawbackDate { get; set; }
        public DateTime? OriginalPaymentDate { get; set; }
    }
}
