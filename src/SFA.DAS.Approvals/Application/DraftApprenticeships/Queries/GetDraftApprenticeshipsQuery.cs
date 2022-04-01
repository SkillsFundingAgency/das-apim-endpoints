using MediatR;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries
{
    public class GetDraftApprenticeshipsQuery : IRequest<GetDraftApprenticeshipsResult>
    {
        public GetDraftApprenticeshipsQuery(long cohortId)
        {
            CohortId = cohortId;
        }

        public long CohortId { get; set; }
    }
}
