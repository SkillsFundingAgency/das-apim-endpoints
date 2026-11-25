using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Commands.AddRestrictedCourse;

public class AddRestrictedCourseCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<AddRestrictedCourseCommandHandler> _logger) : IRequestHandler<AddRestrictedCourseCommand>
{
    public async Task Handle(AddRestrictedCourseCommand command, CancellationToken cancellationToken)
    {
        var apiRequest = new AddRestrictedCourseRequest(command);

        _logger.LogInformation("Handling request to add restricted course for larscode: {LarsCode} by user: {UserId}", command.LarsCode, command.UserId);

        var response = await _courseManagementApiClient.PostWithResponseCode<Unit>(apiRequest);

        response.EnsureSuccessStatusCode();
    }
}
