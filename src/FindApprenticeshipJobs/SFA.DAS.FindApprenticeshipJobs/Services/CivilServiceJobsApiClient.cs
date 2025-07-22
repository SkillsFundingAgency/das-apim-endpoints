using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.Services;
public class CivilServiceJobsApiClient : ICivilServiceJobsApiClient
{
    private readonly CivilServiceJobsConfiguration _apiConfiguration;
    private readonly HttpClient _httpClient;

    public CivilServiceJobsApiClient(IHttpClientFactory httpClientFactory,
        CivilServiceJobsConfiguration apiConfiguration)
    {
        _apiConfiguration = apiConfiguration;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(apiConfiguration.Url);
    }

    public async Task<ApiResponse<string>> GetWithResponseCode(IGetApiRequest request)
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, request.GetUrl);
        httpRequestMessage.Headers.TryAddWithoutValidation("X-API-Key", _apiConfiguration.ApiKey);

        using var response = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
        var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        string? errorContent = null;
        string? responseBody = null;

        if (IsNot200RangeResponseCode(response.StatusCode))
        {
            errorContent = stringResponse;
        }
        else if (!string.IsNullOrWhiteSpace(stringResponse))
        {
            responseBody = stringResponse;
        }

        return new ApiResponse<string>(
            responseBody,
            response.StatusCode,
            errorContent,
            response.Headers.ToDictionary(h => h.Key, h => h.Value)
        );
    }

    private static bool IsNot200RangeResponseCode(HttpStatusCode statusCode)
    {
        return !((int)statusCode >= 200 && (int)statusCode <= 299);
    }
}