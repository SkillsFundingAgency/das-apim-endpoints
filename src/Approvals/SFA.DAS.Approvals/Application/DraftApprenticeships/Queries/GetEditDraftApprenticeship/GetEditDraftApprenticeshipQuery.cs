using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeship
{
    public class GetEditDraftApprenticeshipQuery : IRequest<GetEditDraftApprenticeshipQueryResult>
    {
        public Party Party { get; set; }
        public long PartyId { get; set; }
        public long CohortId { get; set; }
        public long DraftApprenticeshipId { get; set; }
    }
}
