using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCourseForProvider;

public class GetAvailableCourseForProviderQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient) : IRequestHandler<GetAvailableCourseForProviderQuery, GetAvailableCourseForProviderQueryResult>
{
    public async Task<GetAvailableCourseForProviderQueryResult> Handle(GetAvailableCourseForProviderQuery request, CancellationToken cancellationToken)
    {
        var result = await _courseManagementApiClient.GetWithResponseCode<GetAvailableCourseForProviderQueryResult>(new GetAvailableCourseForProviderRequest(request.Ukprn, request.CourseType, request.LarsCode));

        if (result.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        result.EnsureSuccessStatusCode();

        return result.Body;
    }
}
