using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Forecasting.InnerApi.Requests;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.Application.Courses.Queries.GetFrameworkCoursesList;

public class GetFrameworkCoursesQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    : IRequestHandler<GetFrameworkCoursesQuery, GetFrameworkCoursesResult>
{
    public async Task<GetFrameworkCoursesResult> Handle(GetFrameworkCoursesQuery request, CancellationToken cancellationToken)
    {
        var frameworks = await coursesApiClient.Get<GetFrameworksListResponse>(new GetFrameworksRequest());

        return new GetFrameworkCoursesResult
        {
            Frameworks = frameworks.Frameworks.Where(f => f.IsActiveFramework && f.FundingPeriods.Count > 0)
        };
    }
}