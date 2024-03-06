using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpProviderModeration.Application.Infrastructure;
using System.Net;

namespace SFA.DAS.RoatpProviderModeration.Application.Provider.Queries.GetProvider;

public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderQueryResult>
{
    private readonly IRoatpV2ApiClient _apiClient;
    private readonly ILogger<GetProviderQueryHandler> _logger;

    public GetProviderQueryHandler(IRoatpV2ApiClient apiClient, ILogger<GetProviderQueryHandler> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<GetProviderQueryResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get Provider request received for ukprn {ukprn}", request.Ukprn);

        var response = await _apiClient.GetProvider(request.Ukprn, cancellationToken);

        switch (response.ResponseMessage.StatusCode)
        {
            case HttpStatusCode.OK:
                return response.GetContent();
            case HttpStatusCode.NotFound:
                return null;
            default:
                _logger.LogError("Response status code does not indicate success: {statusCode} - Provider details not found for ukprn: {ukprn}", (int)response.ResponseMessage.StatusCode, request.Ukprn);
                throw new InvalidOperationException($"Response status code does not indicate success: {(int)response.ResponseMessage.StatusCode} - Provider details not found for ukprn: {request.Ukprn}");
        }
    }
}
