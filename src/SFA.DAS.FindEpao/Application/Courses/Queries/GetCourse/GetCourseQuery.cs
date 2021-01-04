using MediatR;

namespace SFA.DAS.FindEpao.Application.Courses.Queries.GetCourse
{
    public class GetCourseQuery : IRequest<GetCourseResult>
    {
        public int CourseId { get; set; }
    }
}