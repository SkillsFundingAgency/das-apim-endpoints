using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class ServiceRequest
    {
        public string TaskId { get; set; }
        public string DecisionReference { get; set; }
        public DateTime? TaskCreatedDate { get; set; }
    }
}
