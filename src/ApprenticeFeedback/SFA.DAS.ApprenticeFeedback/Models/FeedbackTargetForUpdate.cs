using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using System;

namespace SFA.DAS.ApprenticeFeedback.Models
{
    public class FeedbackTargetForUpdate
    {
        public Guid Id { get; set; }
        public Guid ApprenticeId { get; set; }

        public static explicit operator FeedbackTargetForUpdate(ApprenticeFeedbackTarget source)
        {
            return new FeedbackTargetForUpdate()
            {
                Id = source.Id,
                ApprenticeId = source.ApprenticeId,
            };
        }
    }
}
