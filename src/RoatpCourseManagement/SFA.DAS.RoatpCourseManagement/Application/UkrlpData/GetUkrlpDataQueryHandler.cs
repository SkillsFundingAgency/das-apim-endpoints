using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.RoatpCourseManagement.Configuration;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models.Ukrlp;

namespace SFA.DAS.RoatpCourseManagement.Application.UkrlpData
{
    public class GetUkrlpDataQueryHandler : IRequestHandler<UkrlpDataQuery, GetUkrlpDataQueryResponse>
    {
        private readonly IUkrlpSoapSerializer _serializer;
        private readonly HttpClient _httpClient;
        private readonly ILogger<GetUkrlpDataQueryHandler> _logger;
        private readonly UkrlpApiConfiguration _ukrlpConfiguration;
        private const int MaximumRecords = 500;

        public GetUkrlpDataQueryHandler(IUkrlpSoapSerializer serializer, HttpClient httpClient,
            ILogger<GetUkrlpDataQueryHandler> logger, IOptions<UkrlpApiConfiguration> options)
        {
            _serializer = serializer;
            _httpClient = httpClient;
            _logger = logger;
            _ukrlpConfiguration = options.Value;
        }

        public async Task<GetUkrlpDataQueryResponse> Handle(UkrlpDataQuery query, CancellationToken cancellationToken)
        {
            if (query.ProvidersUpdatedSince != null)
            {
                return await ProcessUkrlpSinceLastUpdated(query);
            }

            var response = new GetUkrlpDataQueryResponse
            {
                Results = new List<ProviderAddress>(),
                Success = true
            };

            var fetched = 0;

            while (true)
            {
                var ukprnsToCheck = query.Ukprns.Skip(fetched).Take(MaximumRecords).ToList();
                if (!ukprnsToCheck.Any()) break;
                fetched += ukprnsToCheck.Count;
                var request = _serializer.BuildGetAllUkrlpsFromUkprnsSoapRequest(ukprnsToCheck,
                    _ukrlpConfiguration.StakeholderId, _ukrlpConfiguration.QueryId);
                var ukprnResponse = await GetUkrlpResponse(request);

                if (ukprnResponse == null || !ukprnResponse.Success)
                {
                    _logger.LogWarning("The response from UKRLP was failure");
                    return new GetUkrlpDataQueryResponse
                    {
                        Results = new List<ProviderAddress>(),
                        Success = false
                    };
                }

                response.Results.AddRange(ukprnResponse.Results);

                Console.WriteLine($"Total fetched {fetched} off {query.Ukprns.Count}");
            }

            _logger.LogInformation("response gathered from ukrlp using ukprns");

            return response;
        }

        private async Task<GetUkrlpDataQueryResponse> ProcessUkrlpSinceLastUpdated(UkrlpDataQuery query)
        {
            var request = _serializer.BuildGetAllUkrlpsUpdatedSinceSoapRequest(
                (DateTime)query.ProvidersUpdatedSince,
                _ukrlpConfiguration.StakeholderId,
                _ukrlpConfiguration.QueryId);

            var response = await GetUkrlpResponse(request);
            _logger.LogInformation("response gathered from ukrlp from UpdatedSince");

            if (response != null && response.Success) return response;
            _logger.LogWarning("The response from UKRLP was failure");
            return null;
        }

        private async Task<GetUkrlpDataQueryResponse> GetUkrlpResponse(string request)
        {
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
            var matchingProviderRecords = _serializer.DeserialiseMatchingProviderRecordsResponse(soapXml);

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
    }
}

