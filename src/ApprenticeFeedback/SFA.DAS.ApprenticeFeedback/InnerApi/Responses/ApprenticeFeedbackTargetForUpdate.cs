using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Responses
{
    public class ApprenticeFeedbackTargetForUpdate
    {
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public Guid ApprenticeId { get; set; }
        public long ApprenticeshipId { get; set; }
    }
}
