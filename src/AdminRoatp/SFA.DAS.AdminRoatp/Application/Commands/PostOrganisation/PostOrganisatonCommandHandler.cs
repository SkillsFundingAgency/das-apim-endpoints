using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.Infrastructure;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Commands.PostOrganisation;

public class PostOrganisatonCommandHandler(IRoatpServiceRestApiClient _roatpServiceApiClient, IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpV2ApiClient, ILogger<PostOrganisatonCommandHandler> _logger) : IRequestHandler<PostOrganisationCommand, HttpStatusCode>
{
    public async Task<HttpStatusCode> Handle(PostOrganisationCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Posting organisation for ukprn {Ukprn}", command.Ukprn);

        HttpResponseMessage response = await _roatpServiceApiClient.PostOrganisation((PostOrganisationRequest)command, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Created) return response.StatusCode;

        var courseTypes = new List<int>();

        if (command.DeliversApprenticeships) courseTypes.Add((int)CourseType.Apprenticeship);
        if (command.DeliversApprenticeshipUnits) courseTypes.Add((int)CourseType.ApprenticeshipUnit);

        var tasks = new List<Task>();

        _logger.LogInformation("Creating courseTypes for Posted organisation with ukprn {Ukprn}", command.Ukprn);
        UpdateCourseTypesModel model = new UpdateCourseTypesModel(courseTypes.ToArray(), command.RequestingUserDisplayName);
        tasks.Add(_roatpServiceApiClient.PutCourseTypes(command.Ukprn, model, cancellationToken));


        if (command.ProviderType == ProviderType.Main)
        {
            _logger.LogInformation("Creating provider in RoatpV2 for Posted organisation with ukprn {Ukprn}", command.Ukprn);
            tasks.Add(_roatpV2ApiClient.PostWithResponseCode<int>(new PostProviderRequest(command)));
        }

        await Task.WhenAll(tasks);

        return response.StatusCode;
    }
}
