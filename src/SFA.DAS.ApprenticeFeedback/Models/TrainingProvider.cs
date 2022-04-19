using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApprenticeFeedback.Models
{
    public class TrainingProvider
    {
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public string ProviderName { get; set; }
        public long Ukprn { get; set; }
        public DateTime DateSubmitted { get; set; }
        public TimeSpan TimeWindow { get; set; }
        public DateTime SignificantDate { get; set; }
        public FeedbackEligibility FeedbackEligibility { get; set; }
    }

    public enum FeedbackEligibility
    {
        Allow,
        Deny_TooSoon,
        Deny_TooLateAfterPassing,
        Deny_TooLateAfterWithdrawing,
        Deny_HasGivenFeedbackRecently,
        Deny_HasGivenFinalFeedback
    }
}
