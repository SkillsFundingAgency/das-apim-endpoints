
using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTransactionsToEmail
{
    public class GetFeedbackTransactionsToEmailQuery : IRequest<IEnumerable<GetFeedbackTransactionsToEmailResponse>>
    {
        public int BatchSize { get; set; }

        public GetFeedbackTransactionsToEmailQuery(int batchSize)
        {
            BatchSize = batchSize;
        }
    }
}
