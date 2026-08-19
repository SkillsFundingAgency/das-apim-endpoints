using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProviderAvailableCourses;

public class GetProviderAvailableCoursesQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<GetProviderAvailableCoursesQueryHandler> _logger) : IRequestHandler<GetProviderAvailableCoursesQuery, GetProviderAvailableCoursesQueryResult>
{
    public async Task<GetProviderAvailableCoursesQueryResult> Handle(GetProviderAvailableCoursesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle GetProviderAvailableCourses for Ukprn {Ukprn} and Course Type {CourseType}", request.Ukprn, request.CourseType);

        var standards = await _courseManagementApiClient.GetWithResponseCode<GetAllStandardsResponse>(new GetAllStandardsRequest(request.CourseType));

        standards.EnsureSuccessStatusCode();

        var providerAllowedCourses = await _courseManagementApiClient.GetWithResponseCode<GetProviderAllowedCoursesResponse>(new GetProviderAllowedCoursesRequest(request.Ukprn, request.CourseType));

        providerAllowedCourses.EnsureSuccessStatusCode();

        return new GetProviderAvailableCoursesQueryResult()
        {
            Courses = standards.Body.Standards
                .Where(s => !providerAllowedCourses.Body.AllowedCourses.Any(c => c.LarsCode == s.LarsCode))
                .Select(s => new ProviderCourseModel
                {
                    LarsCode = s.LarsCode,
                    Title = s.Title,
                    Level = s.Level
                })
                .ToList()
        };
    }
}
