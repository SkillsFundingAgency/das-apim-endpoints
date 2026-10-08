using System.Net;
using System.Web;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpOversight.Infrastructure;
using SFA.DAS.RoatpOversight.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpOversight.Application.Commands.CreateProvider;

public class CreateProviderCommandHandler : IRequestHandler<CreateProviderCommand, Unit>
{
    private readonly IRoatpV2ApiClient _apiClient;
    private readonly ILogger<CreateProviderCommandHandler> _logger;

    public CreateProviderCommandHandler(IRoatpV2ApiClient apiClient, ILogger<CreateProviderCommandHandler> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateProviderCommand command, CancellationToken cancellationToken)
    {
        var providerGetResponse = await _apiClient.GetProvider(command.Ukprn);

        if (providerGetResponse.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogInformation("Creating provider for ukprn: {Ukprn}", command.Ukprn);
            var response =
                await _apiClient.CreateProvider(HttpUtility.UrlEncode(command.UserId),
                    HttpUtility.UrlEncode(command.UserDisplayName), command, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                _logger.LogError(
                    "Create provider for ukprn: {Ukprn} did not come back with successful response, statusCode:{StatusCode}",
                    command.Ukprn, response.StatusCode);
                throw new InvalidOperationException(
                    $"Create provider for ukprn: {command.Ukprn} did not come back with successful response, statusCode: {response.StatusCode}");
            }

            var addCourseTypesCommand = new AddCourseTypesModel()
            {
                CourseTypes = [CourseType.Apprenticeship],
                UserId = command.UserId,
                UserDisplayName = command.UserDisplayName
            };
            var addCourseTypesResponse = await _apiClient.AddCourseTypes(command.Ukprn, addCourseTypesCommand, cancellationToken);
            addCourseTypesResponse.EnsureSuccessStatusCode();
        }

        return Unit.Value;
    }
}