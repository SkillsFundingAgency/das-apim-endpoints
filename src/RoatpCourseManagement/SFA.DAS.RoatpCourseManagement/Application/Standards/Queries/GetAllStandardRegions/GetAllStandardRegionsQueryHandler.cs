using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;


namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllStandardRegions
{
    public class GetAllStandardRegionsQueryHandler : IRequestHandler<GetAllStandardRegionsQuery, GetAllStandardRegionsQueryResult>
    {
        private readonly ILogger<GetAllStandardRegionsQueryHandler> _logger;
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;
        public GetAllStandardRegionsQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient, ILogger<GetAllStandardRegionsQueryHandler> logger)
        {
            _courseManagementApiClient = courseManagementApiClient;
            _logger = logger;
        }
        public async Task<GetAllStandardRegionsQueryResult> Handle(GetAllStandardRegionsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get All Regions request received");
            var response = await _courseManagementApiClient.GetWithResponseCode<List<StandardRegionModel>>(new GetAllRegionsRequest());
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = $"Response status code does not indicate success: {(int)response.StatusCode} - Regions data not found";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            var regions = response.Body;

            _logger.LogInformation("Get Provider Course Locations request received for ukprn {ukprn} and larsCode {larsCode}", request.Ukprn, request.LarsCode);
            var providerCourseLocationsResponse = await _courseManagementApiClient.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(new GetProviderCourseLocationsRequest(request.Ukprn, request.LarsCode));
            if (providerCourseLocationsResponse.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage =
                   $"Response status code does not indicate success: {(int)providerCourseLocationsResponse.StatusCode} - Provider Course Locations not found for ukprn: {request.Ukprn} LarsCode: {request.LarsCode}";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
            var providerCourseLocations = providerCourseLocationsResponse.Body;

            if (providerCourseLocations == null)
            {
                var message = $"Provider Course Locations found for ukprn {request.Ukprn} and LarsCode {request.LarsCode}";
                _logger.LogError(message);
                throw new ValidationException(message);
            }
            var subRegionCourseLocations = providerCourseLocations.Where(a => a.LocationType == LocationType.Regional).ToList();
            if (subRegionCourseLocations.Any())
            {
                foreach (var region in regions)
                {
                    region.IsSelected = subRegionCourseLocations.Exists(r => r.RegionId == region.Id);
                }
            }

            return new GetAllStandardRegionsQueryResult
            {
                Regions = regions
            };
        }
    }
}
