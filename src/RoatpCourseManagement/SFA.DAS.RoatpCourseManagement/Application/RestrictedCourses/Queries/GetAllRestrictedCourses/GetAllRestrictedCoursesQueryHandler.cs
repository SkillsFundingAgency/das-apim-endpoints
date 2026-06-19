using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.RestrictedCourses.Queries.GetAllRestrictedCourses;

public class GetAllRestrictedCoursesQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<GetAllRestrictedCoursesQueryHandler> _logger) : IRequestHandler<GetAllRestrictedCoursesQuery, GetAllRestrictedCoursesResponse>
{
    public async Task<GetAllRestrictedCoursesResponse> Handle(GetAllRestrictedCoursesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAllRestrictedCourses request");

        var response = await _courseManagementApiClient.GetWithResponseCode<GetAllRestrictedCoursesResponse>(new GetAllRestrictedCoursesRequest(request.Restricted));

        response.EnsureSuccessStatusCode();

        return response.Body;
    }
}
