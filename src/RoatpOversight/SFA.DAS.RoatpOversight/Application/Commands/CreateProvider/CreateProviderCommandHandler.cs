using System.Net;
using System.Web;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpOversight.Infrastructure;

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

        if (!providerGetResponse.IsSuccessStatusCode)
        {
            _logger.LogInformation("Creating provider for ukprn: {ukprn}", command.Ukprn);
            var response =
                await _apiClient.CreateProvider(HttpUtility.UrlEncode(command.UserId),
                    HttpUtility.UrlEncode(command.UserDisplayName), command, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                _logger.LogError(
                    "Create provider for ukprn: {ukprn} did not come back with successful response, statusCode:{statusCode}",
                    command.Ukprn, response.StatusCode);
                throw new InvalidOperationException(
                    $"Create provider for ukprn: {command.Ukprn} did not come back with successful response, statusCode: {response.StatusCode}");
            }
        }

        return Unit.Value;
    }
}