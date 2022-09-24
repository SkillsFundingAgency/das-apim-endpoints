
using System.Collections.Generic;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;


namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTransactionsToEmail
{
    public class GetFeedbackTransactionsToEmailResult
    {
        public IEnumerable<GetFeedbackTransactionsToEmailResponse> FeedbackTransactionsToEmail { get; set; }
    }
}
