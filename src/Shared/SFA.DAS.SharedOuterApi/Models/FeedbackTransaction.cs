using System;

namespace SFA.DAS.SharedOuterApi.Models
{
    public class FeedbackTransaction
    {
        public long ApprenticeFeedbackTransactionId { get; set; }
        public Guid ApprenticeId { get; set; }
        public Guid ApprenticeFeedbackTargetId { get; set; }
    }
}
