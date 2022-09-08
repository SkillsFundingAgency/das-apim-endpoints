using MediatR;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries
{
    public class GetOverlapRequestQuery : IRequest<GetOverlapRequestResult>
    {
        public long DraftApprenticeshipId { get; set; }
    }
}
