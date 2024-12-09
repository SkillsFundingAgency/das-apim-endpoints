using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.Application.Courses.Queries.GetStandardCoursesList;

public class GetStandardCoursesQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    : IRequestHandler<GetStandardCoursesQuery, GetStandardCoursesResult>
{
    public async Task<GetStandardCoursesResult> Handle(GetStandardCoursesQuery request, CancellationToken cancellationToken)
    {
        var standards = await coursesApiClient.Get<GetStandardsListResponse>(new GetAvailableToStartStandardsListRequest());

        return new GetStandardCoursesResult
        {
            Standards = standards.Standards
        };
    }
}