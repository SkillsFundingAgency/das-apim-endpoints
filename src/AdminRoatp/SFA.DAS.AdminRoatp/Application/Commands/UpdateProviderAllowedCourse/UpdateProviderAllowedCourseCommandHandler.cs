using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Commands.UpdateProviderAllowedCourse;

public class UpdateProviderAllowedCourseCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient) : IRequestHandler<UpdateProviderAllowedCourseCommand>
{
    public async Task Handle(UpdateProviderAllowedCourseCommand command, CancellationToken cancellationToken)
    {
        var apiRequest = new PutProviderAllowedCourseRequest(command.Ukprn, command.LarsCode, command.RequestModel);

        var response = await _courseManagementApiClient.PutWithResponseCode<PutProviderAllowedCourseRequestModel, object>(apiRequest);

        response.EnsureSuccessStatusCode();
    }
}
