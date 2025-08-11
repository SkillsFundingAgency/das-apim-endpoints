using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.FindApprenticeshipJobs.Services;
public class CivilServiceJobsApiClient : ICivilServiceJobsApiClient
{
    private readonly ILogger<CivilServiceJobsApiClient> _logger;
    private readonly CivilServiceJobsConfiguration _apiConfiguration;
    private readonly HttpClient _httpClient;

    public CivilServiceJobsApiClient(
        ILogger<CivilServiceJobsApiClient> logger,
        IHttpClientFactory httpClientFactory,
        CivilServiceJobsConfiguration apiConfiguration)
    {
        _logger = logger;
        _apiConfiguration = apiConfiguration;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(apiConfiguration.Url);
    }

    public async Task<ApiResponse<string>> GetWithResponseCode(IGetApiRequest request)
    {
        try
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, request.GetUrl);
            httpRequestMessage.Headers.TryAddWithoutValidation("x-api-key", _apiConfiguration.ApiKey);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _logger.LogInformation($" CSJ GET {request.GetUrl}");
            _logger.LogInformation($"CSJ x-api-key: {_apiConfiguration.ApiKey}");



            using var response = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            _logger.LogInformation("CSJ Response headers:");
            foreach (var header in response.Headers)
            {
                _logger.LogInformation($"CSJ {header.Key}: {string.Join(", ", header.Value)}");
            }
            _logger.LogInformation($"CSJ Body: {stringResponse}");

            var headers = response.Headers
                .Concat(response.Content.Headers)
                .ToDictionary(h => h.Key, h => h.Value);

            return new ApiResponse<string>(
                response.IsSuccessStatusCode ? stringResponse : null,
                response.StatusCode,
                response.IsSuccessStatusCode ? null : stringResponse,
                headers
            );
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(
                null,
                HttpStatusCode.InternalServerError,
                ex.Message,
                new Dictionary<string, IEnumerable<string>>()
            );
        }
    }
}