using MediatR;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipCourse
{
    public class GetEditDraftApprenticeshipCourseQuery : IRequest<GetEditDraftApprenticeshipCourseQueryResult>
    {
        public long CohortId { get; set; }
        public long DraftApprenticeshipId { get; set; }
    }
}
