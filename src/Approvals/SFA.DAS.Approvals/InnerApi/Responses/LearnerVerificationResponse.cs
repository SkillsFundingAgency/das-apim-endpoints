using System.Collections.Generic;
using SFA.DAS.Approvals.Enums;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class LearnerVerificationResponse
    {
        public LearnerVerificationResponseType ResponseType { get; set; }
        public IEnumerable<LearnerDetailMatchingError>? MatchingErrors { get; set; }
    }
}