using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.RoatpCourseManagement.Application.UkrlpData;
using SFA.DAS.RoatpCourseManagement.Configuration;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models.Ukrlp;

namespace SFA.DAS.RoatpCourseManagement.Services
{
    public class UkrlpService: IUkrlpService
    {
        private readonly IUkrlpSoapSerializer _serializer;
        private readonly HttpClient _httpClient;
        private readonly ILogger<UkrlpService> _logger; 
        private readonly UkrlpApiConfiguration _ukrlpConfiguration;

        public UkrlpService(HttpClient httpClient, ILogger<UkrlpService> logger, IUkrlpSoapSerializer serializer, IOptions<UkrlpApiConfiguration> options)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serializer = serializer;
            _ukrlpConfiguration = options.Value;
        }

        public async Task<List<ProviderAddress>> GetAddresses(UkrlpDataCommand command)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            string request;

            if (command.ProvidersUpdatedSince != null)
                request = _serializer.BuildGetAllUkrlpsUpdatedSinceSoapRequest((DateTime)command.ProvidersUpdatedSince, _ukrlpConfiguration.StakeholderId,
                    _ukrlpConfiguration.QueryId);
            else
            {
                request = _serializer.BuildGetAllUkrlpsFromUkprnsSoapRequest(command.Ukprns,
                    _ukrlpConfiguration.StakeholderId, _ukrlpConfiguration.QueryId);
            }

            var response = await GetUkprnLookupResponse(request);
            _logger.LogInformation("response gathered from ukrlp");

            if (response != null  && response.Success)
            {
                return response.Results.Select(GetProviderAddressFromProviderDetails).ToList();

            }

            _logger.LogWarning("The response from UKRLP was failure");
            return null;

        }

        static ProviderAddress GetProviderAddressFromProviderDetails(ProviderDetails providerDetails)
        {
            const string legalContactDetailsIdentifier = "L"; // equivalent to 'Legal Address'

            var legalContactAddress = providerDetails.ContactDetails.FirstOrDefault(c => c.ContactType == legalContactDetailsIdentifier)!.ContactAddress;

            var providerAddress = new ProviderAddress
            {
                Address1 = legalContactAddress.Address1,
                Address2 = legalContactAddress.Address2,
                Address3 = legalContactAddress.Address3,
                Address4 = legalContactAddress.Address4,
                Postcode = legalContactAddress.PostCode,
                Town = legalContactAddress.Town,
                Ukprn = int.Parse(providerDetails.UKPRN)
            };
            return providerAddress;
        }

        private async Task<UkprnLookupResponse> GetUkprnLookupResponse(string request)
        {
            var requestMessage =
                new HttpRequestMessage(HttpMethod.Post, _httpClient.BaseAddress)
                {
                    Content = new StringContent(request, Encoding.UTF8, "text/xml")
                };

            var responseMessage = await _httpClient.SendAsync(requestMessage);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var failureResponse = new UkprnLookupResponse
                {
                    Success = false,
                    Results = new List<ProviderDetails>()
                };
                return await Task.FromResult(failureResponse);
            }

            var soapXml = await responseMessage.Content.ReadAsStringAsync();
            var matchingProviderRecords = _serializer.DeserialiseMatchingProviderRecordsResponse(soapXml);

            if (matchingProviderRecords != null)
            {
                var result = matchingProviderRecords.Select(Mapper.Map<ProviderDetails>).ToList();

                var resultsFound = new UkprnLookupResponse
                {
                    Success = true,
                    Results = result
                };
                return await Task.FromResult(resultsFound);
            }
            else
            {
                var noResultsFound = new UkprnLookupResponse
                {
                    Success = true,
                    Results = new List<ProviderDetails>()
                };
                return await Task.FromResult(noResultsFound);
            }
        }
    }
}
