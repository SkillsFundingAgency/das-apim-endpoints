using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Commands.RestrictProvider;

public class RestrictProviderCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient) : IRequestHandler<RestrictProviderCommand>
{
    public async Task Handle(RestrictProviderCommand command, CancellationToken cancellationToken)
    {
        var model = new RestrictProviderModel()
        {
            UserId = command.UserId,
            UserDisplayName = command.UserDisplayName
        };

        var apiRequest = new RestrictProviderRequest(command.Ukprn, command.CourseType, model);

        var response = await _courseManagementApiClient.PostWithResponseCode<Unit>(apiRequest);

        response.EnsureSuccessStatusCode();
    }
}
