using System;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class UpdateEmploymentCheckRequest
    {
        public Guid CorrelationId { get; set; }
        public string Result { get; set; }
        public DateTime DateChecked { get; set; }
    }
}
