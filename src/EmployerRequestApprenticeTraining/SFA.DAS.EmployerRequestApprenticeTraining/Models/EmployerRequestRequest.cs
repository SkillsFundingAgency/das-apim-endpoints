using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Models
{
    public class CancelEmployerRequestRequest
    {
        public Guid CancelledBy { get; set; }
        public string DashboardUrl { get; set; }
    }
}
