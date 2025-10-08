using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;

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