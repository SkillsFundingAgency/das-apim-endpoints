﻿
using System;


namespace SFA.DAS.ApprenticeFeedback.InnerApi.Responses
{
    public class GetFeedbackTransactionsToEmailResponse
    {
        public long FeedbackTransactionId { get; set; }
        public Guid ApprenticeId  { get; set; }
        public Guid ApprenticeFeedbackTargetId { get; set; }
    }
}
