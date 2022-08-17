using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAllProviderLocations;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourseLocation;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAvailableProviderLocations
{
    public class GetAvailableProviderLocationsQueryHandler : IRequestHandler<GetAvailableProviderLocationsQuery, GetAvailableProviderLocationsQueryResult>
    {
        private readonly ILogger<GetAvailableProviderLocationsQueryHandler> _logger;
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;

        public GetAvailableProviderLocationsQueryHandler(ILogger<GetAvailableProviderLocationsQueryHandler> logger, IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient)
        {
            _logger = logger;
            _courseManagementApiClient = courseManagementApiClient;
        }

        public async Task<GetAvailableProviderLocationsQueryResult> Handle(GetAvailableProviderLocationsQuery request, CancellationToken cancellationToken)
        {
            var allProviderLocationsTask =  _courseManagementApiClient.Get<List<ProviderLocationModel>>(new GetAllProviderLocationsQuery(request.Ukprn));
            var allProviderCourseLocationsTask =  _courseManagementApiClient.Get<List<GetProviderCourseLocationsResponse>>(new GetProviderCourseLocationsRequest(request.Ukprn, request.LarsCode));

            await Task.WhenAll(allProviderLocationsTask, allProviderCourseLocationsTask);
            var allProviderLocations = allProviderLocationsTask.Result.FindAll(a => a.LocationType == LocationType.Provider);
            var allProviderCourseLocations = allProviderCourseLocationsTask.Result;

            _logger.LogInformation($"Retrieved Provider locations:{allProviderLocations.Count} Provider course locations: {allProviderCourseLocations.Count} for ukprn {request.Ukprn} larscode {request.LarsCode} from Roatp API");

            var availableProviderLocations = new List<ProviderLocationModel>();
            foreach (var l in allProviderLocations)
            {
                if (!allProviderCourseLocations.Exists(a => a.LocationName == l.LocationName))
                {
                    availableProviderLocations.Add(l);
                }
            }

            return new GetAvailableProviderLocationsQueryResult() { AvailableProviderLocations = availableProviderLocations };
        }
    }
}
