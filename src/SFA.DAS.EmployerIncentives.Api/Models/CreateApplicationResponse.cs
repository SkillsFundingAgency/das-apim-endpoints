using System;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class CreateApplicationResponse
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
