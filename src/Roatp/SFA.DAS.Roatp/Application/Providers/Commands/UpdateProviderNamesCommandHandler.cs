using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.Domain.Models;
using SFA.DAS.Roatp.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Ukrlp;

namespace SFA.DAS.Roatp.Application.Providers.Commands;

public class
    UpdateProviderNamesCommandHandler : IRequestHandler<UpdateProviderNamesCommand>
{
    private readonly IRoatpApiClient _roatpApiClient;
    private readonly HttpClient _httpClient;
    private readonly IUkrlpSoapSerializer _ukrlpSoapSerializer;
    private readonly UkrlpApiConfiguration _ukrlpConfiguration;
    private readonly ILogger<UpdateProviderNamesCommandHandler> _logger;
    private const int MaximumRecords = 500;

    public UpdateProviderNamesCommandHandler(IRoatpApiClient roatpApiClient, ILogger<UpdateProviderNamesCommandHandler> logger,
        IUkrlpSoapSerializer ukrlpSoapSerializer, UkrlpApiConfiguration ukrlpConfiguration, HttpClient httpClient)
    {
        _roatpApiClient = roatpApiClient;
        _httpClient = httpClient;
        _ukrlpSoapSerializer = ukrlpSoapSerializer;
        _ukrlpConfiguration = ukrlpConfiguration;
        _logger = logger;
    }

    public async Task Handle(UpdateProviderNamesCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting organisations");
        var organisations = _roatpApiClient.GetOrganisations().Result;

        List<long> ukprns = organisations.Organisations.Select(x => (long)x.Ukprn).ToList();

        _logger.LogInformation("Getting Ukrlp data");

        var ukrlpData = new GetUkrlpDataQueryResponse
        {
            Results = new List<ProviderAddress>(),
            Success = true
        };

        var fetched = 0;

        var chunks = ukprns.Chunk(MaximumRecords);

        foreach (var batch in chunks)
        {
            var ukprnResponse = await GetUkrlpResponse(batch.ToList());

            if (ukprnResponse == null || !ukprnResponse.Success)
            {
                _logger.LogWarning("The response from UKRLP was failure");
                return;
            }

            await ProcessNameUpdates(ukprnResponse.Results, organisations);
            ukrlpData.Results.AddRange(ukprnResponse.Results);
        }
    }


    private async Task<GetUkrlpDataQueryResponse> GetUkrlpResponse(List<long> ukprnsToCheck)
    {
        var request = _ukrlpSoapSerializer.BuildGetAllUkrlpsFromUkprnsSoapRequest(ukprnsToCheck,
            _ukrlpConfiguration.StakeholderId, _ukrlpConfiguration.QueryId);

        var requestMessage =
            new HttpRequestMessage(HttpMethod.Post, _ukrlpConfiguration.ApiBaseAddress)
            {
                Content = new StringContent(request, Encoding.UTF8, "text/xml")
            };

        var responseMessage = await _httpClient.SendAsync(requestMessage, CancellationToken.None);

        if (!responseMessage.IsSuccessStatusCode)
        {
            var failureResponse = new GetUkrlpDataQueryResponse
            {
                Success = false,
                Results = new List<ProviderAddress>()
            };
            return await Task.FromResult(failureResponse);
        }

        var soapXml = await responseMessage.Content.ReadAsStringAsync(CancellationToken.None);
        var matchingProviderRecords = _ukrlpSoapSerializer.DeserialiseMatchingProviderRecordsResponse(soapXml);

        if (matchingProviderRecords != null)
        {
            var result = matchingProviderRecords.Select(matchingProvider => (ProviderAddress)matchingProvider)
                .ToList();

            var resultsFound = new GetUkrlpDataQueryResponse
            {
                Success = true,
                Results = result
            };
            return await Task.FromResult(resultsFound);
        }

        var noResultsFound = new GetUkrlpDataQueryResponse
        {
            Success = true,
            Results = new List<ProviderAddress>()
        };
        return await Task.FromResult(noResultsFound);
    }

    private async Task ProcessNameUpdates(List<ProviderAddress> ukrlpData, GetOrganisationsQueryResult organisations)
    {
        foreach (var ukrlp in ukrlpData)
        {
            var parsedUkprn = int.Parse(ukrlp.Ukprn);

            var provider = organisations.Organisations.FirstOrDefault(x => x.Ukprn == parsedUkprn);

            if (provider != null &&
                (!string.Equals(provider.LegalName, ukrlp.ProviderName) ||
                 !string.Equals(provider.TradingName ?? "", ukrlp.TradingName ?? "")))
            {
                _logger.LogInformation("Updating organisation name for ukprn {Ukprn}", provider.Ukprn);

                UpdateOrganisationModel model = new UpdateOrganisationModel
                {
                    ProviderType = provider.ProviderType,
                    OrganisationTypeId = provider.OrganisationTypeId,
                    CharityNumber = provider.CharityNumber,
                    CompanyNumber = provider.CompanyNumber,
                    LegalName = ukrlp.ProviderName ?? "",
                    TradingName = ukrlp.TradingName ?? "",
                    RequestingUserId = "System"
                };

                await _roatpApiClient.PutOrganisation(parsedUkprn, model);
            }
        }
    }
}