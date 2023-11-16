using System;
using static SFA.DAS.ApprenticeFeedback.Models.Enums;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.TrackEmailTransactionClick
{
    public class TrackEmailTransactionClickResult
    {
        public long FeedbackTransactionId { get; set; }
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public string ClickStatus { get; set; }
    }
}
