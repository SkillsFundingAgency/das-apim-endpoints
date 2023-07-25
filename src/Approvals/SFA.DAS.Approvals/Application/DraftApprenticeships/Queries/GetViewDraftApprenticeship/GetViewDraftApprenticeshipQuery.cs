using MediatR;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetViewDraftApprenticeship
{
    public class GetViewDraftApprenticeshipQuery : IRequest<GetViewDraftApprenticeshipQueryResult>
    {
        public long CohortId { get; set; }
        public long DraftApprenticeshipId { get; set; }
    }
}
