using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.CreateProviderLocation
{
    public class AddProviderCourseLocationCommandHandler : IRequestHandler<AddProviderCourseLocationCommand, Unit>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;
        private readonly ILogger<AddProviderCourseLocationCommandHandler> _logger;

        public AddProviderCourseLocationCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient, ILogger<AddProviderCourseLocationCommandHandler> logger)
        {
            _courseManagementApiClient = courseManagementApiClient;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddProviderCourseLocationCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new ProviderCourseLocationCreateRequest(request);
            var response = await _courseManagementApiClient.PostWithResponseCode<int>(apiRequest);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                _logger.LogError("Create provider course location for ukprn: {ukprn} larsCode: {larsCode} locationNavigationId :{locationNavigationId} did not come back with successful response",  request.Ukprn, request.LarsCode, request.LocationNavigationId);
                throw new InvalidOperationException($"Create provider course location did not come back with successful response for ukprn {request.Ukprn} larsCode {request.LarsCode} locationNavigationId {request.LocationNavigationId}");
            }
            return Unit.Value;
        }
    }
}