using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.CreateProviderCourse
{
    public class CreateProviderCourseCommandHandler : IRequestHandler<CreateProviderCourseCommand, Unit>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;
        private readonly ILogger<CreateProviderCourseCommandHandler> _logger;

        public CreateProviderCourseCommandHandler(ILogger<CreateProviderCourseCommandHandler> logger, IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient)
        {
            _logger = logger;
            _courseManagementApiClient = courseManagementApiClient;
        }

        public async Task<Unit> Handle(CreateProviderCourseCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new ProviderCourseCreateRequest(request);
            var response = await _courseManagementApiClient.PostWithResponseCode<int>(apiRequest);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                _logger.LogError("Create provider course :{larscode} for ukprn: {ukprn} did not come back with successful response", request.LarsCode, request.Ukprn);
                throw new InvalidOperationException($"Create provider course: {request.LarsCode} did not come back with successful response for ukprn: {request.Ukprn}");
            }
            return Unit.Value;
        }
    }
}
