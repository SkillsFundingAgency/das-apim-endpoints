using MediatR;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.CheckReviewApprenticeshipCourse
{
    public class CheckReviewApprenticeshipCourseQuery : IRequest<CheckReviewApprenticeshipCourseQueryResult>
    {
        public long ApprenticeshipId { get; set; }
        public string CourseCode { get; set; }
    }
}
