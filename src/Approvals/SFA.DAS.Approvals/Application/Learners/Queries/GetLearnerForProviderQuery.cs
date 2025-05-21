using MediatR;

namespace SFA.DAS.Approvals.Application.Learners.Queries
{
    public class GetLearnerForProviderQuery : IRequest<GetLearnerForProviderQueryResult>
    {
        public long ProviderId { get; set; }
        public long LearnerId { get; set; }
    }
}
