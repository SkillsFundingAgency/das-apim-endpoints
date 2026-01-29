using MediatR;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetEmailNudgeAccountsBatch
{
    public class GetEmailNudgeAccountsBatchQuery : IRequest<GetEmailNudgeAccountsBatchResult>
    {
        public GetEmailNudgeAccountsBatchQuery(int batchSize)
        {
            BatchSize = batchSize;
        }

        public int BatchSize { get; }
    }
}