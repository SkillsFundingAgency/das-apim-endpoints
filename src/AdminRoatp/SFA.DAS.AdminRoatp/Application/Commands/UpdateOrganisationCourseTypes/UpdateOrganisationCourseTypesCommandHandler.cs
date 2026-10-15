using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Commands.UpdateOrganisationCourseTypes;

public class UpdateOrganisationCourseTypesCommandHandler(IRoatpServiceApiClient<RoatpConfiguration> _roatpV1ApiClient, IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCMApiClient, ILogger<UpdateOrganisationCourseTypesCommandHandler> _logger) : IRequestHandler<UpdateOrganisationCourseTypesCommand>
{
    public async Task Handle(UpdateOrganisationCourseTypesCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Update course types request received for Ukprn {Ukprn}", command.Ukprn);

        await UpdateRoatpV1(command);

        await UpdateRoatpCM(command);
    }

    private async Task UpdateRoatpCM(UpdateOrganisationCourseTypesCommand command)
    {
        var courseTypeNames = command.CourseTypeIds.Select(x => (CourseType)x).ToArray();
        var addCourseTypesCommand = new AddCourseTypesModel()
        {
            CourseTypes = courseTypeNames,
            UserId = command.UserId,
            UserDisplayName = command.UserDisplayName
        };
        var cmResponse = await _roatpCMApiClient.PostWithResponseCode<NullResponse>(new AddCourseTypesRequest(command.Ukprn, addCourseTypesCommand));
        cmResponse.EnsureSuccessStatusCode();
    }

    private async Task UpdateRoatpV1(UpdateOrganisationCourseTypesCommand command)
    {
        var updateCourseTypes = new UpdateCourseTypesModel(command.CourseTypeIds, command.UserDisplayName);
        var request = new UpdateCourseTypesRequest(command.Ukprn, updateCourseTypes);
        var v1Response = await _roatpV1ApiClient.PutWithResponseCode<NullResponse>(request);
        v1Response.EnsureSuccessStatusCode();
    }
}
