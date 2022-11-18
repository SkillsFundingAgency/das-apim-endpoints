using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Responses
{
    public class ApprenticeFeedbackTarget
    {
        public Guid Id { get; set; }
        public Guid ApprenticeId { get; set; }
        public long ApprenticeshipId { get; set; }
        public FeedbackTargetStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? LastFeedbackCompletedDate { get; set; }
        public long? Ukprn { get; set; }
        public string ProviderName { get; set; }
        public string StandardUId { get; set; }
        public string StandardName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public enum FeedbackTargetStatus
        {
            Unknown = 0,
            NotYetActive = 1,
            Active = 2,
            Complete = 3,
        }
        public bool Withdrawn { get; set; }
    }
}
