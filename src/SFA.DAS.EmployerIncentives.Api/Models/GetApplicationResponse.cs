using System;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class GetApplicationResponse
    {
        public Guid ApplicationId { get; set; }
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public ApplicationApprenticeshipDto[] Apprentices { get; set; }
    }
}
