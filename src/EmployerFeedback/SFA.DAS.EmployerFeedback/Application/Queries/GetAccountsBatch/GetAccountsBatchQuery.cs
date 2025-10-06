using MediatR;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetAccountsBatch
{
    public class GetAccountsBatchQuery : IRequest<GetAccountsBatchResult>
    {
        public GetAccountsBatchQuery(int batchSize)
        {
            BatchSize = batchSize;
        }

        public int BatchSize { get; }
    }
}