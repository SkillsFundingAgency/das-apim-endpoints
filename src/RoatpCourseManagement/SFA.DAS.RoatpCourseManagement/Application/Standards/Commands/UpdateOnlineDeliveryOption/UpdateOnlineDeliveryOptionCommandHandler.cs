using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateOnlineDeliveryOption;
public class UpdateOnlineDeliveryOptionCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient, ILogger<UpdateOnlineDeliveryOptionCommand> _logger) : IRequestHandler<UpdateOnlineDeliveryOptionCommand, Unit>
{
    public async Task<Unit> Handle(UpdateOnlineDeliveryOptionCommand command, CancellationToken cancellationToken)
    {
        var providerCourseUpdateModel = new ProviderCourseUpdateModel
        {
            Ukprn = command.Ukprn,
            LarsCode = command.LarsCode,
            UserId = command.UserId,
            UserDisplayName = command.UserDisplayName,
            HasOnlineDeliveryOption = command.HasOnlineDeliveryOption
        };

        var patchRequest = new PatchProviderCourseRequest(providerCourseUpdateModel);
        var response = await _innerApiClient.PatchWithResponseCode(patchRequest);
        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            _logger.LogError("Update approved by regulator for larscode:{LarsCode} ukprn: {Ukprn} did not come back with successful response", command.LarsCode, command.Ukprn);
            throw new InvalidOperationException($"Update approved by regulator for larscode: {command.LarsCode} did not come back with successful response for ukprn: {command.Ukprn}");
        }
        return Unit.Value;
    }
}
