using SFA.DAS.ApprenticeFeedback.Models;
using System;
using static SFA.DAS.ApprenticeFeedback.Models.Enums;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeTrainingProvider
{
    public class GetApprenticeTrainingProviderResult
    {
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public string ProviderName { get; set; }
        public long Ukprn { get; set; }
        public DateTime? LastFeedbackSubmittedDate { get; set; }
        public TimeSpan? TimeWindow { get; set; }
        public DateTime? SignificantDate { get; set; }
        public FeedbackEligibility FeedbackEligibility { get; set; }

    }
}
