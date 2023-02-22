using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateContactDetails
{
    public class UpdateContactDetailsCommandHandler : IRequestHandler<UpdateContactDetailsCommand, Unit>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;
        private readonly ILogger<UpdateContactDetailsCommandHandler> _logger;
        public UpdateContactDetailsCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient, ILogger<UpdateContactDetailsCommandHandler> logger)
        {
            _innerApiClient = innerApiClient;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateContactDetailsCommand command, CancellationToken cancellationToken)
        {
            var patchUpdateProviderCourse = new ProviderCourseUpdateModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                UserDisplayName = command.UserDisplayName,
                ContactUsEmail = command.ContactUsEmail,
                ContactUsPhoneNumber = command.ContactUsPhoneNumber,
                ContactUsPageUrl = command.ContactUsPageUrl,
                StandardInfoUrl = command.StandardInfoUrl
            };
    
            var patchRequest = new PatchProviderCourseRequest(patchUpdateProviderCourse);
            var response =  await _innerApiClient.PatchWithResponseCode(patchRequest);
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                _logger.LogError("Update provider course details for larscode:{larscode} ukprn: {ukprn} did not come back with successful response", command.LarsCode, command.Ukprn);
                throw new InvalidOperationException($"Update provider course details for larscode: {command.LarsCode} did not come back with successful response for ukprn: {command.Ukprn}");
            }
            return Unit.Value;
        }
    }
}
