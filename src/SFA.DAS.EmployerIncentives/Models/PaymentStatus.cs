using System;

namespace SFA.DAS.EmployerIncentives.Models
{
    public class PaymentStatus
    {
        public decimal? PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool LearnerMatchFound { get; set; }
        public bool HasDataLock { get; set; }
        public bool InLearning { get; set; }
        public bool PausePayments { get; set; }
        public bool PaymentSent { get; set; }
        public bool PaymentSentIsEstimated { get; set; }
        public bool RequiresNewEmployerAgreement { get; set; }
        public bool PaymentIsStopped { get; set; }
        public bool WithdrawnByEmployer { get; set; }
        public bool WithdrawnByCompliance { get; set; }
        public bool? EmploymentCheckPassed { get; set; }
    }
}
