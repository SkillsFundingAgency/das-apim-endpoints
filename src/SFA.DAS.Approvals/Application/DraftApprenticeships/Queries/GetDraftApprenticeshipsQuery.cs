using MediatR;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries
{
    public class GetDraftApprenticeshipsQuery : IRequest<GetDraftApprenticeshipsResult>
    {
        public long CohortId { get; set; }
    }
}
