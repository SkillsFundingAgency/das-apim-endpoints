using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider
{
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
            var allStandardsTask = _courseManagementApiClient.Get<GetAllStandardsResponse>(new GetAllStandardsRequest());
            var allProviderCoursesTask = _courseManagementApiClient.Get<List<GetAllProviderCoursesResponse>>(new GetAllCoursesRequest(request.Ukprn));

            await Task.WhenAll(allStandardsTask, allProviderCoursesTask);
            var allStandards = allStandardsTask.Result.Standards;
            var allProviderCourses = allProviderCoursesTask.Result;

            _logger.LogInformation($"Retrieved standards:{allStandards.Count} courses: {allProviderCourses.Count} from Roatp API");

            var existingLarsCodes = allProviderCourses.Select(p => p.LarsCode).ToList();

            var availableStandards = allStandards.Where(s => !existingLarsCodes.Contains(s.LarsCode)).Select(c => (AvailableCourseModel)c);
            return new GetAvailableCoursesForProviderQueryResult() { AvailableCourses = availableStandards.ToList() };
        }
    }
}
