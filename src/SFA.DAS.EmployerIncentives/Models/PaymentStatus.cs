using System;

namespace SFA.DAS.EmployerIncentives.Models
{
    public class PaymentStatus
    {
        public decimal? PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool LearnerMatchNotFound { get; set; }
        public bool HasDataLock { get; set; }
    }
}
