using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Commands.PatchProviderAllowedCourse;

public class PatchProviderAllowedCourseCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient) : IRequestHandler<PatchProviderAllowedCourseCommand>
{
    public async Task Handle(PatchProviderAllowedCourseCommand command, CancellationToken cancellationToken)
    {
        var apiRequest = new PatchProviderAllowedCourseRequest(command);

        var response = await _courseManagementApiClient.PatchWithResponseCode(apiRequest);

        response.EnsureSuccessStatusCode();
    }
}
