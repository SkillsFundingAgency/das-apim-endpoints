using System;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class UpdateApplicationRequest
    {
        public Guid ApplicationId { get; set; }
        public long AccountId { get; set; }
        public long[] ApprenticeshipIds { get; set; }
    }
}
