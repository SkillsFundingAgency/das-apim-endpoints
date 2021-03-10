using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindEpao.InnerApi.Requests;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseList
{
    public class GetCourseListQueryHandler : IRequestHandler<GetCourseListQuery, GetCourseListResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetCourseListQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetCourseListResult> Handle(GetCourseListQuery request, CancellationToken cancellationToken)
        {
            var courses = await _coursesApiClient.Get<GetStandardsListResponse>(new GetActiveStandardsListRequest());
            
            return new GetCourseListResult
            {
                Courses = courses.Standards
            }; 
        }
    }
}