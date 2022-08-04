using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using System;

namespace SFA.DAS.ApprenticeFeedback.Models
{
    public class FeedbackTargetForUpdate
    {
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public long ApprenticeshipId { get; set; }

        public static explicit operator FeedbackTargetForUpdate(ApprenticeFeedbackTarget source)
        {
            return new FeedbackTargetForUpdate()
            {
                ApprenticeFeedbackTargetId = source.Id,
                ApprenticeshipId = source.ApprenticeshipId,
            };
        }
    }
}
