using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Courses.Queries.GetCourse
{
    public class GetCourseQuery : IRequest<GetCourseResult>
    {
        public int CourseId { get; set; }
    }
}