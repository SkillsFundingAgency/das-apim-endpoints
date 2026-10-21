using System.Net;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Requests.SldDataApi;

namespace SFA.DAS.LearnerData.Services;

public interface IEnglishAndMathsValueService
{
    Task<decimal> GetValue(string learnAimRef, DateTime startDate);
}

// Gets the value of an English and Maths course from SLD, which is what its earnings are calculated from
public class EnglishAndMathsValueService : IEnglishAndMathsValueService
{
    private static readonly TimeSpan[] DefaultRetryDelays = [TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(8)];

    private readonly ISldDataApiClient<SLDDataApiConfiguration> _sldDataApiClient;
    private readonly ILogger<EnglishAndMathsValueService> _logger;
    private readonly AsyncRetryPolicy<ApiResponse<decimal?>> _retryPolicy;

    // retryDelays is only for tests, so they don't wait between attempts
    public EnglishAndMathsValueService(
        ISldDataApiClient<SLDDataApiConfiguration> sldDataApiClient,
        ILogger<EnglishAndMathsValueService> logger,
        TimeSpan[]? retryDelays = null)
    {
        _sldDataApiClient = sldDataApiClient;
        _logger = logger;

        // Only failures that might clear up on their own are retried; a 4xx or a missing value won't change on another attempt
        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .OrResult<ApiResponse<decimal?>>(response => IsServerError(response.StatusCode))
            .WaitAndRetryAsync(retryDelays ?? DefaultRetryDelays,
                onRetry: (outcome, delay, attempt, _) =>
                    logger.LogWarning(outcome.Exception,
                        "SLD English and Maths value transient failure ({StatusCode}) on attempt {Attempt}. Waiting {Delay}s before the next attempt.",
                        outcome.Result?.StatusCode, attempt, delay.TotalSeconds));
    }

    public async Task<decimal> GetValue(string learnAimRef, DateTime startDate)
    {
        var request = new GetEnglishAndMathsValueRequest(learnAimRef, DateOnly.FromDateTime(startDate));

        ApiResponse<decimal?> response;
        try
        {
            response = await _retryPolicy.ExecuteAsync(() => _sldDataApiClient.GetWithResponseCode<decimal?>(request));
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
        {
            _logger.LogError(exception, "Could not reach SLD for the English and Maths value of course {LearnAimRef} starting {StartDate:yyyy-MM-dd}", learnAimRef, startDate);
            throw new EnglishAndMathsValueUnavailableException(
                $"Could not reach SLD for the English and Maths value of course {learnAimRef} starting {startDate:yyyy-MM-dd}.", exception);
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError("SLD has no English and Maths value for course {LearnAimRef} starting {StartDate:yyyy-MM-dd}. Response: {ErrorContent}", learnAimRef, startDate, response.ErrorContent);
            throw new EnglishAndMathsValueNotFoundException(learnAimRef, startDate);
        }

        if (response.StatusCode != HttpStatusCode.OK || response.Body == null)
        {
            _logger.LogError("SLD did not return an English and Maths value for course {LearnAimRef} starting {StartDate:yyyy-MM-dd}. Status code {StatusCode}, response: {ErrorContent}", learnAimRef, startDate, (int)response.StatusCode, response.ErrorContent);
            throw new EnglishAndMathsValueUnavailableException(
                $"SLD did not return an English and Maths value for course {learnAimRef} starting {startDate:yyyy-MM-dd}. Status code {(int)response.StatusCode}.", response.StatusCode);
        }

        return response.Body.Value;
    }

    private static bool IsServerError(HttpStatusCode statusCode) => (int)statusCode >= 500;
}
