using System;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class CreateDraftSubmissionRequest
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long[] ApprenticeshipIds { get; set; }
    }
}
