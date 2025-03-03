using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using System;

namespace SFA.DAS.ApprenticeFeedback.Models
{
    public class FeedbackTargetVariant
    {
        public long ApprenticeshipId { get; set; }
        public string? Variant { get; set; }
    }
}
