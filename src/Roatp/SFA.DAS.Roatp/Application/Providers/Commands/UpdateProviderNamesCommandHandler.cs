using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.Domain.Models;
using SFA.DAS.Roatp.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;

namespace SFA.DAS.Roatp.Application.Providers.Commands;

public class UpdateProviderNamesCommandHandler : IRequestHandler<UpdateProviderNamesCommand>
{
    private readonly IRoatpApiClient _roatpApiClient;
    private readonly ILogger<UpdateProviderNamesCommandHandler> _logger;

    public UpdateProviderNamesCommandHandler(IRoatpApiClient roatpApiClient, ILogger<UpdateProviderNamesCommandHandler> logger)
    {
        _roatpApiClient = roatpApiClient;
        _logger = logger;
    }

    public async Task Handle(UpdateProviderNamesCommand command, CancellationToken cancellationToken)
    {
        GetOrganisationsResponse result = await _roatpApiClient.GetOrganisations(cancellationToken);
        Dictionary<int, OrganisationResponse> organisations = result.Organisations.ToDictionary(x => x.Ukprn);

        IEnumerable<int> ukprns = result.Organisations.Select(x => x.Ukprn);

        _logger.LogInformation("Getting Ukrlp data");

        const int MaximumUkprnsPerRequest = 100;
        var chunks = ukprns.Chunk(MaximumUkprnsPerRequest);

        var sharedBatchNumber = 0;
        var chunkCount = chunks.Count();
        var totalProvidersUpdated = 0;

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 4,
            CancellationToken = cancellationToken
        };

        await Parallel.ForEachAsync(chunks, parallelOptions, async (batch, ct) =>
        {
            int currentBatch = Interlocked.Increment(ref sharedBatchNumber);
            _logger.LogInformation("Processing batch of ukprns: {BatchNumber} of {TotalBatches}", currentBatch, chunkCount);

            UkrlpProvidersResponse ukprnResponse = await _roatpApiClient.GetProvidersDataFromUkrlp(null, batch, ct);
            var count = await ProcessNameUpdates(ukprnResponse, organisations, ct);

            _logger.LogInformation("Updated {Count} providers in {BatchNumber}", count, currentBatch);
            Interlocked.Add(ref totalProvidersUpdated, count);
        });

        _logger.LogInformation("Total providers updated {TotalProvidersUpdated}.", totalProvidersUpdated);
    }

    private async Task<int> ProcessNameUpdates(UkrlpProvidersResponse ukrlpData, Dictionary<int, OrganisationResponse> organisationsLookup, CancellationToken cancellationToken)
    {
        List<(int Ukprn, UpdateOrganisationModel Model)> updatesToSend = [];
        foreach (var ukrlp in ukrlpData.Providers)
        {
            var parsedUkprn = ukrlp.Ukprn;

            if (!organisationsLookup.TryGetValue(ukrlp.Ukprn, out var provider))
            {
                continue;
            }

            var hasNameChanged = !string.Equals(provider.LegalName, ukrlp.LegalName, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(provider.TradingName, ukrlp.TradingName, StringComparison.OrdinalIgnoreCase);

            if (hasNameChanged)
            {
                _logger.LogInformation("Organisation with ukprn {Ukprn} require name updated", provider.Ukprn);

                UpdateOrganisationModel model = new()
                {
                    ProviderType = provider.ProviderType,
                    OrganisationTypeId = provider.OrganisationTypeId,
                    CharityNumber = provider.CharityNumber,
                    CompanyNumber = provider.CompanyNumber,
                    LegalName = ukrlp.LegalName,
                    TradingName = ukrlp.TradingName,
                    RequestingUserId = "System"
                };

                updatesToSend.Add((parsedUkprn, model));
            }
        }

        const int maxConcurrentRequests = 10;
        foreach (var batch in updatesToSend.Chunk(maxConcurrentRequests))
        {
            IEnumerable<Task> tasks = batch.Select(update => _roatpApiClient.PutOrganisation(update.Ukprn, update.Model, cancellationToken));
            await Task.WhenAll(tasks);
        }
        return updatesToSend.Count;
    }
}
