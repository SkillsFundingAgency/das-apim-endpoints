using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApprenticeFeedback.Models
{
    public class Enums
    {
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
            Deny_NotEnoughActiveApprentices = 8,
            Deny_Complete = 9,
        }
    }
}
