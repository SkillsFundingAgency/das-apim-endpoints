using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Application.Regions.Queries
{
    public class GetAllRegionsQueryHandler : IRequestHandler<GetAllRegionsQuery, GetAllRegionsQueryResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;
        private readonly ILogger<GetAllRegionsQueryHandler> _logger;
        public GetAllRegionsQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient, ILogger<GetAllRegionsQueryHandler> logger)
        {
            _courseManagementApiClient = courseManagementApiClient;
            _logger = logger;
        }

        public async Task<GetAllRegionsQueryResult> Handle(GetAllRegionsQuery request, CancellationToken cancellationToken)
        {
            var response = await _courseManagementApiClient.GetWithResponseCode<List<RegionModel>>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = $"Response status code does not indicate success: {(int)response.StatusCode} - Regions data not found";
                _logger.LogError(errorMessage);
                throw new HttpRequestContentException(errorMessage, response.StatusCode, response.ErrorContent);
            }
            return new GetAllRegionsQueryResult() { Regions = response.Body };
        }
    }
}
