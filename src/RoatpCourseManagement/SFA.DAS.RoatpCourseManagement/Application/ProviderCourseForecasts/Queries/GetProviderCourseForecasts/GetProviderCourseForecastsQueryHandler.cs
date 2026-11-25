using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Queries.GetProviderCourseForecasts;

public class GetProviderCourseForecastsQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient) : IRequestHandler<GetProviderCourseForecastsQuery, GetProviderCourseForecastsQueryResult>
{
    public async Task<GetProviderCourseForecastsQueryResult> Handle(GetProviderCourseForecastsQuery request, CancellationToken cancellationToken)
    {
        ApiResponse<GetProviderCourseForecastsQueryResult> response = await _courseManagementApiClient.GetWithResponseCode<GetProviderCourseForecastsQueryResult>(new GetProviderCourseForecastsRequest(request.Ukprn, request.LarsCode));
        response.EnsureSuccessStatusCode();
        return response.Body;
    }
}
