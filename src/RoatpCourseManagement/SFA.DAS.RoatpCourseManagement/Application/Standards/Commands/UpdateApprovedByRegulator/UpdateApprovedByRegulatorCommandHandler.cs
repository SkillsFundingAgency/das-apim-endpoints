using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateApprovedByRegulator
{
    public class UpdateApprovedByRegulatorCommandHandler : IRequestHandler<UpdateApprovedByRegulatorCommand, Unit>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;
        private readonly ILogger<UpdateApprovedByRegulatorCommand> _logger;

        public UpdateApprovedByRegulatorCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient, ILogger<UpdateApprovedByRegulatorCommand> logger)
        {
            _innerApiClient = innerApiClient;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateApprovedByRegulatorCommand command, CancellationToken cancellationToken)
        {
            var providerCourseUpdateModel = new ProviderCourseUpdateModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                UserDisplayName = command.UserDisplayName,
                IsApprovedByRegulator = command.IsApprovedByRegulator
            };

            var patchRequest = new PatchProviderCourseRequest(providerCourseUpdateModel);
            var response = await _innerApiClient.PatchWithResponseCode(patchRequest);
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                _logger.LogError("Update approved by regulator for larscode:{larscode} ukprn: {ukprn} did not come back with successful response", command.LarsCode, command.Ukprn);
                throw new InvalidOperationException($"Update approved by regulator for larscode: {command.LarsCode} did not come back with successful response for ukprn: {command.Ukprn}");
            }
            return Unit.Value;
        }
    }
}
