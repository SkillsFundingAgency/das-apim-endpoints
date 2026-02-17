using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetApprenticeshipIdsByCommitmentId
{
    public class GetApprenticeshipIdsByCommitmentIdQuery : IRequest<GetApprenticeshipIdsByCommitmentIdQueryResult>
    {
        public long CommitmentId { get; set; }
    }
}
