using System.Net;
using MediatR;
using SFA.DAS.AdminRoatp.Application.Commands.PatchProviderAllowedCourse;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Commands.UpdateProviderRestrictedApprenticeship;

public class UpdateProviderRestrictedApprenticeshipCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient) : IRequestHandler<UpdateProviderRestrictedApprenticeshipCommand>
{
    public async Task Handle(UpdateProviderRestrictedApprenticeshipCommand command, CancellationToken cancellationToken)
    {
        var providerAllowedCourseResponse = await _courseManagementApiClient.GetWithResponseCode<GetProviderAllowedCourseDetailsResponse?>(new GetProviderAllowedCourseDetailsRequest(command.Ukprn, command.LarsCode));

        providerAllowedCourseResponse.EnsureSuccessStatusCode();

        if (providerAllowedCourseResponse.StatusCode == HttpStatusCode.NoContent)
        {
            var model = new AddProviderAllowedCourseModel()
            {
                UserId = command.UserId,
                UserDisplayName = command.UserDisplayName,
                LastDateStarts = command.LastDateStarts,
                IsStartRestricted = false,
            };

            var apiRequest = new AddProviderAllowedCourseRequest(command.Ukprn, command.LarsCode, model);

            var response = await _courseManagementApiClient.PostWithResponseCode<Unit>(apiRequest);

            response.EnsureSuccessStatusCode();
        }

        if (providerAllowedCourseResponse.StatusCode == HttpStatusCode.OK)
        {
            var patchCommand = new PatchProviderAllowedCourseCommand()
            {
                UserId = command.UserId,
                UserDisplayName = command.UserDisplayName,
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                LastDateStarts = command.LastDateStarts
            };

            var apiRequest = new PatchProviderAllowedCourseRequest(patchCommand);

            var response = await _courseManagementApiClient.PatchWithResponseCode(apiRequest);

            response.EnsureSuccessStatusCode();
        }
    }
}
