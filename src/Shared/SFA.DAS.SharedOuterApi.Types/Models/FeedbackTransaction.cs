namespace SFA.DAS.SharedOuterApi.Types.Models
{
    public class FeedbackTransaction
    {
        public long FeedbackTransactionId { get; set; }
        public Guid ApprenticeId { get; set; }
        public Guid ApprenticeFeedbackTargetId { get; set; }
    }
}
