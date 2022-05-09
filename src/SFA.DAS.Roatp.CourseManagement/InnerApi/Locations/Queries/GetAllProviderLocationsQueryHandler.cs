using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Locations.Queries
{
    public class GetAllProviderLocationsQueryHandler : IRequestHandler<GetAllProviderLocationsQuery, GetAllProviderLocationsQueryResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;
        private readonly ILogger<GetAllProviderLocationsQueryHandler> _logger;

        public GetAllProviderLocationsQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient, ILogger<GetAllProviderLocationsQueryHandler> logger)
        {
            _courseManagementApiClient = courseManagementApiClient;
            _logger = logger;
        }

        public async Task<GetAllProviderLocationsQueryResult> Handle(GetAllProviderLocationsQuery request, CancellationToken cancellationToken)
        {
            var response = await _courseManagementApiClient.Get<List<ProviderLocationModel>>(request);
            return new GetAllProviderLocationsQueryResult() { ProviderLocations = response };
        }
    }
}
