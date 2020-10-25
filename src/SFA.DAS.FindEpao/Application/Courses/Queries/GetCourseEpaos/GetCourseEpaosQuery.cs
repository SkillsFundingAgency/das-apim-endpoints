using MediatR;

namespace SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpaos
{
    public class GetCourseEpaosQuery : IRequest<GetCourseEpaosResult>
    {
        public int CourseId { get; set; }
    }
}