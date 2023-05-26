using MediatR;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipPriorLearningSummary
{
    public class GetEditDraftApprenticeshipPriorLearningSummaryQuery : IRequest<GetEditDraftApprenticeshipPriorLearningSummaryQueryResult>
    {
        public GetEditDraftApprenticeshipPriorLearningSummaryQuery(long cohortId, long draftApprenticeshipId)
        {
            CohortId = cohortId;
            DraftApprenticeshipId = draftApprenticeshipId;
        }

        public long CohortId { get; set; }
        public long DraftApprenticeshipId { get; set; }
    }
}
