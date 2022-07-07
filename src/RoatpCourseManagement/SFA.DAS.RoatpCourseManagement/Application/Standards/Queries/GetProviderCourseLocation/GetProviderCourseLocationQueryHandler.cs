using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourseLocation
{
    public class GetProviderCourseLocationQueryHandler : IRequestHandler<GetProviderCourseLocationQuery,GetProviderCourseLocationResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;
        private readonly ILogger<GetProviderCourseLocationQueryHandler> _logger;

        public GetProviderCourseLocationQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient, ILogger<GetProviderCourseLocationQueryHandler> logger)
        {
            _courseManagementApiClient = courseManagementApiClient;
            _logger = logger;
        }

        public async Task<GetProviderCourseLocationResult> Handle(GetProviderCourseLocationQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Provider Course locations request received for ukprn {ukprn}, LarsCode {larsCode}", request.Ukprn, request.LarsCode);

            var providerCourseLocationsResponse = await _courseManagementApiClient.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(new GetProviderCourseLocationsRequest(request.Ukprn, request.LarsCode));
            if (providerCourseLocationsResponse.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage =
                   $"Response status code does not indicate success: {(int)providerCourseLocationsResponse.StatusCode} - Provider Course locations not found for ukprn: {request.Ukprn} LarsCode: {request.LarsCode}";
                _logger.LogError(errorMessage);
                throw new HttpRequestContentException(errorMessage, providerCourseLocationsResponse.StatusCode, providerCourseLocationsResponse.ErrorContent);
            }
            var providerCourseLocations = providerCourseLocationsResponse.Body;

            var locations = providerCourseLocations.Select(x => (ProviderCourseLocationModel)x).ToList();
            return new GetProviderCourseLocationResult
            {
                ProviderCourseLocations = locations,
            };
        }
    }
}
