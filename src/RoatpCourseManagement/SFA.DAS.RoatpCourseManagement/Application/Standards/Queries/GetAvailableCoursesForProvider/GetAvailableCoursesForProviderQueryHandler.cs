using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider;

public class GetAvailableCoursesForProviderQueryHandler : IRequestHandler<GetAvailableCoursesForProviderQuery, GetAvailableCoursesForProviderQueryResult>
{
    private readonly ILogger<GetAvailableCoursesForProviderQueryHandler> _logger;
    private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;

    public GetAvailableCoursesForProviderQueryHandler(ILogger<GetAvailableCoursesForProviderQueryHandler> logger, IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient)
    {
        _logger = logger;
        _courseManagementApiClient = courseManagementApiClient;
    }

    public async Task<GetAvailableCoursesForProviderQueryResult> Handle(GetAvailableCoursesForProviderQuery request, CancellationToken cancellationToken)
    {
        var allStandardsTask = _courseManagementApiClient.Get<GetAllStandardsResponse>(new GetAllCoursesRequest(request.CourseType));
        var allProviderCoursesTask = _courseManagementApiClient.Get<List<GetAllProviderCoursesResponse>>(new GetAllProviderCoursesRequest(request.Ukprn, request.CourseType));

        await Task.WhenAll(allStandardsTask, allProviderCoursesTask);

        List<GetStandardResponse> allStandards = allStandardsTask.Result.Standards;
        List<GetAllProviderCoursesResponse> allProviderCourses = allProviderCoursesTask.Result;

        _logger.LogInformation("Retrieved standards:{StandardCount} courses: {CoursesCount} from Roatp API", allStandards.Count, allProviderCourses.Count);

        var existingLarsCodes = allProviderCourses.Select(p => p.LarsCode).ToList();

        var availableStandards = allStandards.Where(s => !existingLarsCodes.Contains(s.LarsCode)).Select(c => (AvailableCourseModel)c);

        availableStandards = await FurtherFilterAvailableCoursesWithAllowedCourses(request, availableStandards);

        return new GetAvailableCoursesForProviderQueryResult(availableStandards);
    }

    private async Task<IEnumerable<AvailableCourseModel>> FurtherFilterAvailableCoursesWithAllowedCourses(GetAvailableCoursesForProviderQuery request, IEnumerable<AvailableCourseModel> availableCourses)
    {
        if (request.CourseType != CourseType.ShortCourse) return availableCourses;
        var allowedCourses = await _courseManagementApiClient.Get<GetAllowedCoursesForProviderResponse>(new GetAllowedCoursesForProviderRequest(request.Ukprn, request.CourseType));

        return availableCourses.Where(s => allowedCourses.AllowedCourses.Any(ac => ac.LarsCode == s.LarsCode));
    }
}
