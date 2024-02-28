using System.Net;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Services;

public class NhsJobsApiClient : INhsJobsApiClient
{
    private readonly HttpClient _httpClient;

    public NhsJobsApiClient(IHttpClientFactory httpClientFactory, NhsJobsConfiguration apiConfiguration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(apiConfiguration.Url);
    }
    
    public async Task<ApiResponse<string>> GetWithResponseCode(IGetApiRequest request)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, request.GetUrl);
        httpRequestMessage.AddVersion(request.Version);

        var response = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

        var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var errorContent = "";
        var responseBody = "";

        if (IsNot200RangeResponseCode(response.StatusCode))
        {
            errorContent = stringResponse;
        }
        else if (string.IsNullOrWhiteSpace(stringResponse))
        {
            // 204 No Content from a potential returned null
            // Will throw if attempts to deserialise but didn't
            // feel right making it part of the error if branch
            // even if there is no content.
        }
        else
        {
            responseBody = stringResponse;
        }

        var getWithResponseCode = new ApiResponse<string>(responseBody, response.StatusCode, errorContent);

        return getWithResponseCode;
    }

    private static bool IsNot200RangeResponseCode(HttpStatusCode statusCode)
    {
        return !((int)statusCode >= 200 && (int)statusCode <= 299);
    }
    
}