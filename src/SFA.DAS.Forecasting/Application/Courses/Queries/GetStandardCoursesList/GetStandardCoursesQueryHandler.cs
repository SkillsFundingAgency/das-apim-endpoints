using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Forecasting.InnerApi.Requests;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.Application.Courses.Queries.GetStandardCoursesList
{
    public class GetStandardCoursesQueryHandler :IRequestHandler<GetStandardCoursesQuery, GetStandardCoursesResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        public GetStandardCoursesQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetStandardCoursesResult> Handle(GetStandardCoursesQuery request, CancellationToken cancellationToken)
        {
            var standards = await _coursesApiClient.Get<GetStandardsListResponse>(new GetAvailableToStartStandardsListRequest());

            return new GetStandardCoursesResult
            {
                Standards = standards.Standards
            };
        }
    }
}