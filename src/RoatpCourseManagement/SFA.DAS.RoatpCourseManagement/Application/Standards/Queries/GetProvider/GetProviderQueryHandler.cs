using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProvider
{
    public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;
        private readonly ILogger<GetProviderQueryHandler> _logger;

        public GetProviderQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient, ILogger<GetProviderQueryHandler> logger)
        {
            _courseManagementApiClient = courseManagementApiClient;
            _logger = logger;
        }

        public async Task<GetProviderResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Provider request received for ukprn {ukprn}", request.Ukprn);

           
            var response = await _courseManagementApiClient.GetWithResponseCode<GetProviderResponse>(new GetProviderRequest(request.Ukprn));
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage =
                   $"Response status code does not indicate success: {(int)response.StatusCode} - Provider details not found for ukprn: {request.Ukprn}";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
            var provider = response.Body;

            return provider;
        }
    }
}
