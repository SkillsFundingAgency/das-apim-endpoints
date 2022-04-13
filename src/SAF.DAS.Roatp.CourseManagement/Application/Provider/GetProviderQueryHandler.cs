using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.Application.Provider
{
    public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _apiClient;
        private readonly ILogger<GetProviderQueryHandler> _logger;

        public GetProviderQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> apiClient, ILogger<GetProviderQueryHandler> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
        public async Task<GetProviderResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get provider request received for Ukprn number {Ukprn}", request.Ukprn);
            try
            {
                var provider = await _apiClient.Get<GetProviderResponse>(new GetProviderRequest(request.Ukprn));

                return new GetProviderResult(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred trying to retrieve provider for Ukprn number {request.Ukprn}");
                throw;
            }

           
        }
    }
}