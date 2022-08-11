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
    public class CreateProviderLocationCommandHandler : IRequestHandler<CreateProviderLocationCommand, Unit>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;
        private readonly ILogger<CreateProviderLocationCommandHandler> _logger;

        public CreateProviderLocationCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient, ILogger<CreateProviderLocationCommandHandler> logger)
        {
            _courseManagementApiClient = courseManagementApiClient;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateProviderLocationCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new ProviderLocationCreateRequest(request);
            var response = await _courseManagementApiClient.PostWithResponseCode<int>(apiRequest);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                _logger.LogError("Create provider location name :{locationname} for ukprn: {ukprn} did not come back with successful response", request.LocationName, request.Ukprn);
                throw new InvalidOperationException($"Create provider location did not come back with successful response for ukprn{request.Ukprn}");
            }
            return Unit.Value;
        }
    }
}