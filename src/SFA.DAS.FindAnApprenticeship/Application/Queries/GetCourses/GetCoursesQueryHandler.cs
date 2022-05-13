using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCourses
{
    public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, GetCoursesQueryResult>
    {
        private readonly ICourseService _courseService;

        public GetCoursesQueryHandler(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public async Task<GetCoursesQueryResult> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            var response =
                await _courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));

            return new GetCoursesQueryResult
            {
                Standards = response.Standards
            };
        }   
    }
}