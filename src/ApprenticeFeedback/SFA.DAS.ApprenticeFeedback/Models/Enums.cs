
namespace SFA.DAS.ApprenticeFeedback.Models
{
    public class Enums
    {
        public enum FeedbackTargetStatus
        {
            Unknown = 0,
            NotYetActive = 1,
            Active = 2,
            Complete = 3,
        }

        public enum FeedbackEligibility
        {
            Unknown = 0,
            Allow = 1,
            Deny_TooSoon = 2,
            Deny_TooLateAfterPassing = 3,
            Deny_TooLateAfterWithdrawing = 4,
            Deny_TooLateAfterPausing = 5,
            Deny_HasGivenFeedbackRecently = 6,
            Deny_HasGivenFinalFeedback = 7,
            Deny_Complete = 9,
        }

        public enum OverallRating
        {
            VeryPoor = 1,
            Poor = 2,
            Good = 3,
            Excellent = 4
        }

        public enum FeedbackAttributeStatus
        {
            Disagree = 0,
            Agree = 1
        }

        public enum EmailStatus
        {
            Successful,
            NotAllowed,
            Failed
        }
    }
}
