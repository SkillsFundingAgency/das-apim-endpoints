using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Application.Regions.Queries;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Regions.Queries
{
    public class GetAllRegionsQueryHandler : IRequestHandler<GetAllRegionsQuery, GetAllRegionsQueryResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;
        public readonly ILogger<GetAllRegionsQueryHandler> _logger;

        public GetAllRegionsQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient, ILogger<GetAllRegionsQueryHandler> logger)
        {
            _courseManagementApiClient = courseManagementApiClient;
            _logger = logger;
        }

        public async Task<GetAllRegionsQueryResult> Handle(GetAllRegionsQuery request, CancellationToken cancellationToken)
        {
            var response = await _courseManagementApiClient.GetWithResponseCode<List<RegionModel>>(new GetAllRegionsRequest());
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = "Get all the regions did not come back with successful status code.";
                _logger.LogError(error);
                throw new InvalidOperationException(error);
            }
            return new GetAllRegionsQueryResult { Regions = response.Body };
        }
    }
}
