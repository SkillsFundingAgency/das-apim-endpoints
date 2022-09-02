using MediatR;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeship
{
    public class GetEditDraftApprenticeshipQuery : IRequest<GetEditDraftApprenticeshipQueryResult>
    {
        public long CohortId { get; set; }
        public long DraftApprenticeshipId { get; set; }
    }
}
