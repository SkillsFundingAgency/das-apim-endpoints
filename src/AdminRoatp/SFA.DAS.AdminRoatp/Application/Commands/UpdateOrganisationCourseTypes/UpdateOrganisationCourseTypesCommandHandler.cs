using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Commands.UpdateOrganisationCourseTypes;
public class UpdateOrganisationCourseTypesCommandHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient, ILogger<UpdateOrganisationCourseTypesCommandHandler> _logger) : IRequestHandler<UpdateOrganisationCourseTypesCommand>
{
    public async Task Handle(UpdateOrganisationCourseTypesCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Update course types request received for Ukprn {Ukprn}", command.ukprn);
        var updateCourseTypes = new UpdateCourseTypesModel(command.CourseTypeIds, command.UserId);
        var request = new UpdateCourseTypesRequest(command.ukprn, updateCourseTypes);

        var response = await _apiClient.PutWithResponseCode<NullResponse>(request);

        response.EnsureSuccessStatusCode();
    }
}