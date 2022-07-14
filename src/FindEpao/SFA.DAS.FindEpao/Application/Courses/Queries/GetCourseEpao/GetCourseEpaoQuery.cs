using MediatR;

namespace SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpao
{
    public class GetCourseEpaoQuery : IRequest<GetCourseEpaoResult>
    {
        public int CourseId { get; set; }
        public string EpaoId { get; set; }
    }
}