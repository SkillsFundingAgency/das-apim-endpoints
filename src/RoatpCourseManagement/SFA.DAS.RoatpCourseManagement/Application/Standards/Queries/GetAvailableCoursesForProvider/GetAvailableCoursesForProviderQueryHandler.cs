using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider;

public class GetAvailableCoursesForProviderQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient) : IRequestHandler<GetAvailableCoursesForProviderQuery, GetAvailableCoursesForProviderQueryResult>
{
    public async Task<GetAvailableCoursesForProviderQueryResult> Handle(GetAvailableCoursesForProviderQuery request, CancellationToken cancellationToken)
    {
        var result = await _courseManagementApiClient.Get<GetAvailableCoursesForProviderQueryResult>(new GetAvailableCoursesForProviderRequest(request.Ukprn, request.CourseType));

        return result;
    }
}
