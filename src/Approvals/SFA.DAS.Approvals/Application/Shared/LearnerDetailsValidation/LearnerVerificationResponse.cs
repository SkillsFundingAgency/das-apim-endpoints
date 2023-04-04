using SFA.DAS.Approvals.Enums;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.Shared.LearnerDetailsValidation
{
    public class LearnerVerificationResponse
    {
        public LearnerVerificationResponseType ResponseType { get; set; }
        public IEnumerable<LearnerDetailMatchingError> MatchingErrors { get; set; }
    }
}