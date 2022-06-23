using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindEpao.Application.Courses.Queries.GetCourse
{
    public class GetCourseQueryHandler : IRequestHandler<GetCourseQuery, GetCourseResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetCourseQueryHandler (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetCourseResult> Handle(GetCourseQuery request, CancellationToken cancellationToken)
        {
            var course = await _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.CourseId));
            
            return new GetCourseResult
            {
                Course = course
            };
        }
    }
}