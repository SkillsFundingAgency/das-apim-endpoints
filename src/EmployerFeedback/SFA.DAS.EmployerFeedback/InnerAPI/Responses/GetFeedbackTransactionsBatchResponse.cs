using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.InnerApi.Responses
{
    public class GetFeedbackTransactionsBatchResponse
    {
        public List<long> FeedbackTransactions { get; set; }
    }
}