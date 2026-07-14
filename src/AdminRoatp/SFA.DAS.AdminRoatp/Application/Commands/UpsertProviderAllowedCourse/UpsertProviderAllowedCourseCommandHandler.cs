using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Commands.UpsertProviderAllowedCourse;

public class UpsertProviderAllowedCourseCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient) : IRequestHandler<UpsertProviderAllowedCourseCommand>
{
    public async Task Handle(UpsertProviderAllowedCourseCommand command, CancellationToken cancellationToken)
    {
        var apiRequest = new UpsertProviderAllowedCourseRequest(command);

        var response = await _courseManagementApiClient.PostWithResponseCode<Unit>(apiRequest);

        response.EnsureSuccessStatusCode();
    }
}
