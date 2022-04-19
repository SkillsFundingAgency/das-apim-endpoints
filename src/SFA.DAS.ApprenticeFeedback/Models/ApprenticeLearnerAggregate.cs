using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.ApprenticeFeedback.Models
{
    public class ApprenticeLearnerAggregate
    {
        public ApprenticeFeedbackTarget FeedbackTarget { get; set; }
        public GetApprenticeLearnerResponse Learner { get; set; }
        public int LearnerCountForProvider { get; set; }
    }
}
