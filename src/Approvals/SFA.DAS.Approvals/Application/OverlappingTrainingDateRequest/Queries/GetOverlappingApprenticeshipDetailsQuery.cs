using MediatR;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries
{
    public class GetOverlappingApprenticeshipDetailsQuery : IRequest<GetOverlappingApprenticeshipDetailsQueryResult>
    {
        public long DraftApprenticeshipId { get; set; }
        public long ProviderId { get; set; }
    }
}
