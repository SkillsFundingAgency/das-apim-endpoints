using System;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class CreateApplicationRequest
    {
        public Guid ApplicationId { get; set; }
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long[] ApprenticeshipIds { get; set; }
    }
}
