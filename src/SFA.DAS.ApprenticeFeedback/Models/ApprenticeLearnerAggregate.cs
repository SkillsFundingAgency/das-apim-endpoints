using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System;

namespace SFA.DAS.ApprenticeFeedback.Models
{
    public class ApprenticeLearnerAggregate
    {
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public GetApprenticeLearnerResponse Learner { get; set; }
    }
}
