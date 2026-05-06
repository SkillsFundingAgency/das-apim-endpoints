using MediatR;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionsBatch
{
    public class GetFeedbackTransactionsBatchQuery : IRequest<GetFeedbackTransactionsBatchResult>
    {
        public GetFeedbackTransactionsBatchQuery(int batchSize)
        {
            BatchSize = batchSize;
        }

        public int BatchSize { get; }
    }
}