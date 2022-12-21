using System;

namespace SFA.DAS.ApprenticeFeedback.Models
{
    public class ApprenticeExitSurvey
    {
        public Guid Id { get; set; }
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public DateTime DateTimeCompleted { get; set; }
        public bool DidNotCompleteApprenticeship { get; set; }
    }
}
