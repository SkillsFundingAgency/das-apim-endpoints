using MediatR;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetEditApprenticeshipCourse
{
    public class GetEditApprenticeshipCourseQuery : IRequest<GetEditApprenticeshipCourseQueryResult>
    {
        public long ApprenticeshipId { get; set; }
    }
}
