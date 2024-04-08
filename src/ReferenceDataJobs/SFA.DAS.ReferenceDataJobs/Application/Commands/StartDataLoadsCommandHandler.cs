using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ReferenceDataJobs.Configuration;
using SFA.DAS.ReferenceDataJobs.InnerApi.Requests;
using SFA.DAS.ReferenceDataJobs.Interfaces;

namespace SFA.DAS.ReferenceDataJobs.Application.Commands;
public class StartDataLoadsCommandHandler : IRequestHandler<StartDataLoadsCommand>
{
    private readonly IPublicSectorOrganisationsApiClient<PublicSectorOrganisationsApiConfiguration> _psoApiClient;
    private readonly ILogger<StartDataLoadsCommandHandler> _logger;

    public StartDataLoadsCommandHandler(IPublicSectorOrganisationsApiClient<PublicSectorOrganisationsApiConfiguration> psoApiClient, ILogger<StartDataLoadsCommandHandler> logger)
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
