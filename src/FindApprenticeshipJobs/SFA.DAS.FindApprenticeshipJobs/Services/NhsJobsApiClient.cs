using System.Net;
using Polly;
using Polly.Retry;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;

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
        var response = await RetryPipeline.ExecuteAsync(async token =>
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, request.GetUrl);
            requestMessage.AddVersion(request.Version);

            return await _httpClient.SendAsync(requestMessage, token);
        });
        
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
            // Will throw if attempts to deserialize but didn't
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

    private static readonly ResiliencePipeline<HttpResponseMessage> RetryPipeline =
        new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                MaxRetryAttempts = 3,

                ShouldHandle = args => ValueTask.FromResult(
                    args.Outcome.Result is not null &&
                    (
                        args.Outcome.Result.StatusCode == HttpStatusCode.TooManyRequests ||
                        args.Outcome.Result.StatusCode == HttpStatusCode.RequestTimeout ||
                        (int)args.Outcome.Result.StatusCode >= 500
                    )),

                DelayGenerator = args =>
                {
                    var retryAfter = args.Outcome.Result?
                        .Headers
                        .RetryAfter?
                        .Delta;

                    return ValueTask.FromResult<TimeSpan?>(
                        retryAfter ?? TimeSpan.FromSeconds(15));
                }
            })
            .Build();

    private static bool IsNot200RangeResponseCode(HttpStatusCode statusCode)
    {
        return !((int)statusCode >= 200 && (int)statusCode <= 299);
    }
}