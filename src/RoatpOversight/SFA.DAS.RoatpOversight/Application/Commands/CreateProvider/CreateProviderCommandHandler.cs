using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpOversight.Application.Commands.CreateProvider;
using SFA.DAS.RoatpOversight.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpOversight.Application.Providers.Commands.CreateProvider;

public class CreateProviderCommandHandler : IRequestHandler<CreateProviderCommand, Unit>
{
    private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _apiClient;
    private readonly ILogger<CreateProviderCommandHandler> _logger;

    public CreateProviderCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> apiClient, ILogger<CreateProviderCommandHandler> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateProviderCommand command, CancellationToken cancellationToken)
    {
        CreateProviderRequest apiRequest = new(command);

        var response = await _apiClient.PostWithResponseCode<int>(apiRequest);

        if (response.StatusCode != HttpStatusCode.Created)
        {
            _logger.LogError("Create provider for ukprn: {ukprn} did not come back with successful response, statusCode:{statusCode}", command.Ukprn, response.StatusCode);
            throw new InvalidOperationException($"Create provider for ukprn: {command.Ukprn} did not come back with successful response, statusCode: {response.StatusCode}");
        }

        return Unit.Value;
    }
}