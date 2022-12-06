using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Requests;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;

namespace SFA.DAS.RoatpProviderModeration.Application.Queries.GetProvider
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
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NotFound)
            {
                _logger.LogError("Response status code does not indicate success: {statusCode} - Provider details not found for ukprn: {ukprn}", (int)response.StatusCode, request.Ukprn);
                throw new InvalidOperationException($"Response status code does not indicate success: {(int)response.StatusCode} - Provider details not found for ukprn: {request.Ukprn}");
            }
            var provider = response?.Body;

            return provider;
        }
    }
}
