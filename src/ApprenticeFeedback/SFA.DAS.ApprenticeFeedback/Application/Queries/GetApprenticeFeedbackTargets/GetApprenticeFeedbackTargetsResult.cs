using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeFeedbackTargets
{
    public class GetApprenticeFeedbackTargetsResult
    {
        public IEnumerable<ApprenticeFeedbackTarget> FeedbackTargets { get; set; }
    }
}
