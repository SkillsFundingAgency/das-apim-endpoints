using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using Microsoft.Extensions.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using System.Net.Http.Headers;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Api.Common.Infrastructure;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsQueryHandler : IRequestHandler<SearchOrganisationsQuery, SearchOrganisationsResult>
    {
        private readonly ILogger<SearchOrganisationsQueryHandler> _logger;
        private readonly IReferenceDataApiClient _apiClient;
        private readonly IReferenceDataApiClient<ReferenceDataApiConfiguration> _refDataApi;
        protected readonly HttpClient HttpClient;
        private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
        public SearchOrganisationsQueryHandler(IHttpClientFactory httpClientFactory,
            IAzureClientCredentialHelper azureClientCredentialHelper,
            ILogger<SearchOrganisationsQueryHandler> logger, IReferenceDataApiClient<ReferenceDataApiConfiguration> referenceDataApiClient, IReferenceDataApiClient apiClient)
        {
            HttpClient = httpClientFactory.CreateClient();
            //HttpClient.BaseAddress = new Uri(apiConfiguration.Url);
            _azureClientCredentialHelper = azureClientCredentialHelper;
            _logger = logger;
            _apiClient = apiClient;
            _refDataApi = referenceDataApiClient;
        }

        public async Task<SearchOrganisationsResult> Handle(SearchOrganisationsQuery request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Searching for Organisation with searchTerm: {SearchTerm}", request.SearchTerm);

            if (request.Version == 0)
            {
                var absoluteOrgs = await GetWithResponseCode<GetSearchOrganisationsResponse>(new GetSearchOrganisationsRequest(request.SearchTerm, request.MaximumResults));

                return new SearchOrganisationsResult(absoluteOrgs.Body);
            }

            var organisations = await _refDataApi.Get<GetSearchOrganisationsResponse>(new GetSearchOrganisationsRequest(request.SearchTerm, request.MaximumResults));

            return new SearchOrganisationsResult(organisations);
        }


        public async Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            Uri uri;
            if (Uri.TryCreate("https://test2-refdata.apprenticeships.education.gov.uk/api/organisations/" + request.GetUrl, UriKind.Absolute, out uri))
            {
                // uri is an absolute URI
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                httpRequestMessage.AddVersion(request.Version);
                await AddAuthenticationHeader(httpRequestMessage);

                Console.WriteLine($"Sending HTTP request: {httpRequestMessage.Method} {httpRequestMessage.RequestUri}");
                if (httpRequestMessage.Headers != null)
                {
                    Console.WriteLine("RequestHEaders: ");
                    foreach (var header in httpRequestMessage.Headers)
                    {
                        Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                    }
                }

                if (httpRequestMessage.Content != null)
                {
                    Console.WriteLine($"Request Content: {await httpRequestMessage.Content.ReadAsStringAsync()}");
                }

                var response = await HttpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var errorContent = "";
                var responseBody = (TResponse)default;

                if (IsNot200RangeResponseCode(response.StatusCode))
                {
                    errorContent = json;
                }
                else if (string.IsNullOrWhiteSpace(json))
                {
                    // 204 No Content from a potential returned null
                    // Will throw if attempts to deserialise but didn't
                    // feel right making it part of the error if branch
                    // even if there is no content.
                }
                else
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    options.Converters.Add(new JsonStringEnumConverter());
                    responseBody = JsonSerializer.Deserialize<TResponse>(json, options);
                }

                var getWithResponseCode = new ApiResponse<TResponse>(responseBody, response.StatusCode, errorContent);

                return getWithResponseCode;
            }

            return null;
        }

        private static bool IsNot200RangeResponseCode(HttpStatusCode statusCode)
        {
            return !((int)statusCode >= 200 && (int)statusCode <= 299);
        }

        private async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync("https://citizenazuresfabisgov.onmicrosoft.com/das-test2-refapi-as-ar");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
