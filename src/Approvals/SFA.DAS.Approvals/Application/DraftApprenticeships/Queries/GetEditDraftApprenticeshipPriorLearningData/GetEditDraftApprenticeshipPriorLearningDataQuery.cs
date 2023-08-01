using MediatR;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipPriorLearningData
{
    public class GetEditDraftApprenticeshipPriorLearningDataQuery : IRequest<GetEditDraftApprenticeshipPriorLearningDataQueryResult>
    {
        public GetEditDraftApprenticeshipPriorLearningDataQuery(long cohortId, long draftApprenticeshipId)
        {
            CohortId = cohortId;
            DraftApprenticeshipId = draftApprenticeshipId;
        }

        public long CohortId { get; set; }
        public long DraftApprenticeshipId { get; set; }
    }
}
