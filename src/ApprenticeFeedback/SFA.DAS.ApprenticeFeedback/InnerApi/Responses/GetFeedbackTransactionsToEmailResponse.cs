
using System;


namespace SFA.DAS.ApprenticeFeedback.InnerApi.Responses
{
    public class GetFeedbackTransactionsToEmailResponse
    {
        public long Id { get; set; }
        public Guid ApprenticeId  { get; set; }
    }
}
