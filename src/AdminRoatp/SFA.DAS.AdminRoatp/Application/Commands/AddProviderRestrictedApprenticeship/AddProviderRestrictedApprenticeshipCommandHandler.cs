using System.Net;
using MediatR;
using SFA.DAS.AdminRoatp.Application.Commands.PatchProviderAllowedCourse;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Commands.AddProviderRestrictedApprenticeship;

public class AddProviderRestrictedApprenticeshipCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient) : IRequestHandler<AddProviderRestrictedApprenticeshipCommand>
{
    private static readonly DateTime StartRestrictedDate = new(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public async Task Handle(AddProviderRestrictedApprenticeshipCommand command, CancellationToken cancellationToken)
    {
        var providerAllowedCourseResponse = await _courseManagementApiClient.GetWithResponseCode<GetProviderAllowedCourseDetailsResponse?>(new GetProviderAllowedCourseDetailsRequest(command.Ukprn, command.LarsCode));

        providerAllowedCourseResponse.EnsureSuccessStatusCode();

        var providerCourseResponse = await _courseManagementApiClient.GetWithResponseCode<GetProviderCourseResponse>(new GetProviderCourseRequest(command.Ukprn, command.LarsCode));

        switch (providerAllowedCourseResponse, providerCourseResponse)
        {
            case ({ StatusCode: HttpStatusCode.NoContent }, { StatusCode: not HttpStatusCode.OK }):
                await CreateProviderAllowedCourse(command, false);
                return;

            case ({ StatusCode: HttpStatusCode.NoContent }, { StatusCode: HttpStatusCode.OK }):
                await CreateProviderAllowedCourse(command, true);
                return;

            case ({ StatusCode: HttpStatusCode.OK, Body.IsCourseRestricted: false }, { StatusCode: not HttpStatusCode.OK }):
                await PatchProviderAllowedCourse(command, false);
                return;

            case ({ StatusCode: HttpStatusCode.OK, Body.IsCourseRestricted: true }, { StatusCode: not HttpStatusCode.OK }):
                await DeleteProviderAllowedCourse(command);
                return;

            case ({ StatusCode: HttpStatusCode.OK }, { StatusCode: HttpStatusCode.OK }):
                await PatchProviderAllowedCourse(command, true);
                return;
        }
    }

    private async Task CreateProviderAllowedCourse(AddProviderRestrictedApprenticeshipCommand command, bool providerCourseExists)
    {
        var model = new AddProviderAllowedCourseModel
        {
            UserId = command.UserId,
            UserDisplayName = command.UserDisplayName,
            LastDateStarts = !providerCourseExists ? null : command.LastDateStarts,
            IsStartRestricted = !providerCourseExists
        };

        var request = new AddProviderAllowedCourseRequest(command.Ukprn, command.LarsCode, model);

        ApiResponse<Unit> response = await _courseManagementApiClient.PostWithResponseCode<Unit>(request);

        response.EnsureSuccessStatusCode();
    }

    private async Task PatchProviderAllowedCourse(AddProviderRestrictedApprenticeshipCommand command, bool providerCourseExists)
    {
        var patchCommand = new PatchProviderAllowedCourseCommand
        {
            UserId = command.UserId,
            UserDisplayName = command.UserDisplayName,
            Ukprn = command.Ukprn,
            LarsCode = command.LarsCode,
            LastDateStarts = !providerCourseExists ? StartRestrictedDate : command.LastDateStarts
        };

        var request = new PatchProviderAllowedCourseRequest(patchCommand);

        ApiResponse<string> response = await _courseManagementApiClient.PatchWithResponseCode(request);

        response.EnsureSuccessStatusCode();
    }

    private async Task DeleteProviderAllowedCourse(AddProviderRestrictedApprenticeshipCommand command)
    {
        var request = new DeleteProviderAllowedCourseRequest(command.Ukprn, command.LarsCode, command.UserId, command.UserDisplayName);

        ApiResponse<Unit> response = await _courseManagementApiClient.DeleteWithResponseCode<Unit>(request);

        response.EnsureSuccessStatusCode();
    }
}
