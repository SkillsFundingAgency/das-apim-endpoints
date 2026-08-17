using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProviderAllowedCourses;

public class GetProviderAllowedCoursesQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<GetProviderAllowedCoursesQueryHandler> _logger) : IRequestHandler<GetProviderAllowedCoursesQuery, GetProviderAllowedCoursesResponse>
{
    public async Task<GetProviderAllowedCoursesResponse> Handle(GetProviderAllowedCoursesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle get provider allowed courses request for Ukprn {Ukprn} and Course Type {CourseType}", request.Ukprn, request.CourseType);

        var response = await _courseManagementApiClient.GetWithResponseCode<GetProviderAllowedCoursesResponse>(new GetProviderAllowedCoursesRequest(request.Ukprn, request.CourseType));

        response.EnsureSuccessStatusCode();

        return response.Body;
    }
}
