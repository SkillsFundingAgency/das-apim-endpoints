using MediatR;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.GetReviewApprenticeshipUpdates
{
    public class GetReviewApprenticeshipUpdatesQuery : IRequest<GetReviewApprenticeshipUpdatesQueryResult>
    {
        public long ApprenticeshipId { get; set; }
    }
}
