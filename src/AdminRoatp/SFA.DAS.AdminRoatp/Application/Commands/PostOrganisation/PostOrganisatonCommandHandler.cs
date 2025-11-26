using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.Application.Commands.CreateProvider;
using SFA.DAS.AdminRoatp.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.AdminRoatp.Application.Commands.PostOrganisation;

public class PostOrganisatonCommandHandler(IRoatpServiceRestApiClient _roatpServiceApiClient, IRoatpV2ApiClient _roatpV2ApiClient, ILogger<PostOrganisatonCommandHandler> _logger) : IRequestHandler<PostOrganisationCommand, HttpStatusCode>
{
    public async Task<HttpStatusCode> Handle(PostOrganisationCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Posting organisation for ukprn {Ukprn}", command.Ukprn);

        HttpResponseMessage response = await _roatpServiceApiClient.PostOrganisation((PostOrganisationModel)command, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Created) return response.StatusCode;

        var courseTypes = new List<int>();

        if (command.DeliversApprenticeships) courseTypes.Add((int)CourseType.Apprenticeships);
        if (command.DeliversApprenticeshipUnits) courseTypes.Add((int)CourseType.ApprenticeshipUnits);

        var tasks = new List<Task>();

        if (courseTypes.Count > 0)
        {
            _logger.LogInformation("Creating courseTypes for Posted organisation with ukprn {Ukprn}", command.Ukprn);
            PutCourseTypesModel model = new PutCourseTypesModel(courseTypes, command.RequestingUserDisplayName);
            tasks.Add(_roatpServiceApiClient.PutCourseTypes(command.Ukprn, model, cancellationToken));
        }

        if (command.ProviderType == ProviderType.Main)
        {
            _logger.LogInformation("Creating provider in RoatpV2 for Posted organisation with ukprn {Ukprn}", command.Ukprn);
            tasks.Add(_roatpV2ApiClient.CreateProvider(command.RequestingUserId,
                 command.RequestingUserDisplayName, (CreateProviderModel)command,
                 cancellationToken));
        }

        if (tasks.Count > 0) await Task.WhenAll(tasks);

        return response.StatusCode;
    }
}
