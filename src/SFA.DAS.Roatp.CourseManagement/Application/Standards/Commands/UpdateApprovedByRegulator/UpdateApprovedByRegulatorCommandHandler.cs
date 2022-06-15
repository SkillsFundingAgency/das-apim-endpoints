using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateApprovedByRegulator
{
    public class UpdateApprovedByRegulatorCommandHandler : IRequestHandler<UpdateApprovedByRegulatorCommand, HttpStatusCode>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;
        private readonly ILogger<UpdateApprovedByRegulatorCommandHandler> _logger;
        public UpdateApprovedByRegulatorCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient, ILogger<UpdateApprovedByRegulatorCommandHandler> logger)
        {
            _innerApiClient = innerApiClient;
            _logger = logger;
        }

        public async Task<HttpStatusCode> Handle(UpdateApprovedByRegulatorCommand command, CancellationToken cancellationToken)
        {
            var providerCourseResponse = await _innerApiClient.GetWithResponseCode<GetProviderCourseResponse>(new GetProviderCourseRequest(command.Ukprn, command.LarsCode));
            if (providerCourseResponse.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage =
                   $"Response status code does not indicate success: {(int)providerCourseResponse.StatusCode} - Provider course details not found for ukprn: {command.Ukprn} LarsCode: {command.LarsCode}";
                _logger.LogError(errorMessage);
                throw new HttpRequestContentException(errorMessage, providerCourseResponse.StatusCode, providerCourseResponse.ErrorContent);
            }
            var providerCourse = providerCourseResponse.Body;
            var updateProviderCourse = new ProviderCourseUpdateModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                ContactUsEmail = providerCourse.ContactUsEmail,
                ContactUsPhoneNumber = providerCourse.ContactUsPhoneNumber,
                ContactUsPageUrl = providerCourse.ContactUsPageUrl,
                StandardInfoUrl = providerCourse.StandardInfoUrl,
                IsApprovedByRegulator = command.IsApprovedByRegulator
            };
            var request = new UpdateProviderCourseRequest(updateProviderCourse);
            var response = await _innerApiClient.PostWithResponseCode<UpdateProviderCourseRequest>(request);
            return response.StatusCode;
        }
    }
}
