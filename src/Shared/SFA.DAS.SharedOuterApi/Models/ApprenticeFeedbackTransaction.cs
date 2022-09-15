
using System;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;


namespace SFA.DAS.SharedOuterApi.Models
{
    public class ApprenticeFeedbackTransaction
    {
        public long ApprenticeFeedbackTransactionId { get; set; }
        public Guid ApprenticeId { get; set; }
        public Guid ApprenticeFeedbackTargetId { get; set; }
    }
}
