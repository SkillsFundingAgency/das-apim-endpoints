using MediatR;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetAddDraftApprenticeshipCourse
{
    public class GetAddDraftApprenticeshipCourseQuery : IRequest<GetAddDraftApprenticeshipCourseQueryResult>
    {
        public long CohortId { get; set; }
        public long DraftApprenticeshipId { get; set; }
    }
}
