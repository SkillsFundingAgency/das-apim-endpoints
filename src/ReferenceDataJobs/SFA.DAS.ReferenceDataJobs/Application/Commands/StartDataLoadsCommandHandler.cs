using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PublicSectorOrganisations;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ReferenceDataJobs.Application.Commands;
public class StartDataLoadsCommandHandler : IRequestHandler<StartDataLoadsCommand>
{
    private readonly IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration> _psoApiClient;
    private readonly ILogger<StartDataLoadsCommandHandler> _logger;

    public StartDataLoadsCommandHandler(IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration> psoApiClient, ILogger<StartDataLoadsCommandHandler> logger)
    {
        _psoApiClient = psoApiClient;
        _logger = logger;
    }

    public async Task Handle(StartDataLoadsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Calling Client to Starting ImportData");
            await _psoApiClient.PostWithResponseCode<object>(new PostPublicSectorOrganisationsDataLoadRequest(), false);
        }
        catch (Exception ex)
        {
            _logger.LogError("Errored when runnning ImportData", ex);
            throw;
        }
    }
}
