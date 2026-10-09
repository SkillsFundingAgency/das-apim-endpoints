using System.Net;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.LearnerData.Requests.CommitmentsInner;
using SFA.DAS.LearnerData.Responses.CommitmentsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Services;

public interface IApprovalsService
{
    /// <summary>
    /// Asks Approvals about the changes in the response. The caller decides whether an ask is needed (Learning signals it
    /// with ChangesNeedingApproval); this always asks.
    /// Returns true only when every change was auto-approved.
    /// Returns false when anything else came back (pending, rejected, unrecognised, or no verdicts at all).
    /// Pending and rejected are deliberately not distinguished: both are simply a lack of approval.
    /// Throws if Approvals could not be reached or returned an error.
    /// </summary>
    Task<bool> RequestApproval(long ukprn, long uln, BaseLearnerApiPutResponse learningApiPutResponse);
}

/// <summary>
/// Asks Approvals whether a change detected on approved learning needs employer approval.
/// We hold no approval rules ourselves: we send the change and act only on the verdicts that come back.
/// </summary>
public class ApprovalsService : IApprovalsService
{
    private const int MaxRetries = 3;
    private const string AutoApprovedStatus = "autoApproved";

    private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsApiClient;
    private readonly IApprovalsRequestBuilder _requestBuilder;
    private readonly ILogger<ApprovalsService> _logger;
    private readonly AsyncRetryPolicy<ApiResponse<ApprovalsResponse>> _retryPolicy;

    public ApprovalsService(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient,
        IApprovalsRequestBuilder requestBuilder,
        ILogger<ApprovalsService> logger)
        : this(commitmentsApiClient, requestBuilder, logger, TimeSpan.FromSeconds(2))
    {
    }

    public ApprovalsService(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient,
        IApprovalsRequestBuilder requestBuilder,
        ILogger<ApprovalsService> logger,
        TimeSpan retryBaseDelay)
    {
        _commitmentsApiClient = commitmentsApiClient;
        _requestBuilder = requestBuilder;
        _logger = logger;

        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .OrResult<ApiResponse<ApprovalsResponse>>(r => IsTransient(r.StatusCode))
            .WaitAndRetryAsync(
                MaxRetries,
                attempt => TimeSpan.FromTicks(retryBaseDelay.Ticks * (1L << (attempt - 1))),
                onRetry: (outcome, delay, attempt, _) =>
                {
                    var failure = outcome.Exception != null
                        ? $"exception: {outcome.Exception.Message}"
                        : $"status {(int)outcome.Result.StatusCode} ({outcome.Result.StatusCode}): {outcome.Result.ErrorContent}";

                    _logger.LogWarning("Approvals call failed transiently ({Failure}). Retry {Attempt} of {MaxRetries} after {Delay}s.",
                        failure, attempt, MaxRetries, delay.TotalSeconds);
                });
    }

    public async Task<bool> RequestApproval(long ukprn, long uln, BaseLearnerApiPutResponse learningApiPutResponse)
    {
        var request = _requestBuilder.Build(ukprn, uln, learningApiPutResponse);
        LogRequest(request.Data);

        ApiResponse<ApprovalsResponse> apiResponse;
        try
        {
            apiResponse = await _retryPolicy.ExecuteAsync(() =>
                _commitmentsApiClient.PutWithResponseCode<ApprovalsRequestBody, ApprovalsResponse>(request));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Approvals request failed for learning {LearningKey} (apprenticeship {ApprenticeshipId}). No approval decision was obtained.",
                request.Data.LearningKey, request.Data.ApprenticeshipId);
            throw;
        }

        if (!IsSuccess(apiResponse.StatusCode))
        {
            _logger.LogError("Approvals responded {StatusCode} for learning {LearningKey} (apprenticeship {ApprenticeshipId}). No approval decision was obtained. Response body: {ErrorContent}",
                (int)apiResponse.StatusCode, request.Data.LearningKey, request.Data.ApprenticeshipId, apiResponse.ErrorContent);

            throw new HttpRequestContentException(
                $"Approvals responded {(int)apiResponse.StatusCode} ({apiResponse.StatusCode}) for learning {request.Data.LearningKey}.",
                apiResponse.StatusCode,
                apiResponse.ErrorContent);
        }

        var allAutoApproved = AreAllAutoApproved(apiResponse.Body);
        LogOutcome(request.Data, apiResponse.Body, allAutoApproved);

        return allAutoApproved;
    }

    private static bool AreAllAutoApproved(ApprovalsResponse? response)
    {
        var verdicts = response?.Changes ?? [];

        return verdicts.Count > 0
               && verdicts.All(v => string.Equals(v.ApprovalStatus, AutoApprovedStatus, StringComparison.OrdinalIgnoreCase));
    }

    private void LogRequest(ApprovalsRequestBody body)
    {
        _logger.LogInformation("Asking Approvals about learning {LearningKey} (apprenticeship {ApprenticeshipId}, ukprn {Ukprn}, type {LearningType}): {ChangeCount} change(s), {PriceCount} price(s)",
            body.LearningKey, body.ApprenticeshipId, body.Ukprn, body.LearningType, body.Changes.Count, body.NewPrices.Count);

        foreach (var change in body.Changes)
        {
            _logger.LogInformation("Approvals request for learning {LearningKey} (apprenticeship {ApprenticeshipId}) - change {ChangeType}: old '{Old}' new '{New}'",
                body.LearningKey, body.ApprenticeshipId, change.ChangeType, change.Data.Old, change.Data.New);
        }

        foreach (var price in body.NewPrices)
        {
            _logger.LogInformation("Approvals request for learning {LearningKey} (apprenticeship {ApprenticeshipId}) - price: training {TrainingPrice}, assessment {AssessmentPrice}, effective from {EffectiveFrom}",
                body.LearningKey, body.ApprenticeshipId, price.TrainingPrice, price.AssessmentPrice, price.EffectiveFrom.ToString("yyyy-MM-dd"));
        }
    }

    private void LogOutcome(ApprovalsRequestBody body, ApprovalsResponse? response, bool allAutoApproved)
    {
        var verdicts = response?.Changes ?? [];

        foreach (var verdict in verdicts)
        {
            _logger.LogInformation("Approvals outcome for learning {LearningKey} (apprenticeship {ApprenticeshipId}) - {ChangeType}: {ApprovalStatus}, reason '{Reason}'",
                body.LearningKey, body.ApprenticeshipId, verdict.ChangeType, verdict.ApprovalStatus, verdict.Reason);
        }

        if (verdicts.Count == 0)
        {
            _logger.LogWarning("Approvals returned no verdicts for learning {LearningKey} (apprenticeship {ApprenticeshipId}). Treating as not auto-approved.",
                body.LearningKey, body.ApprenticeshipId);
        }

        _logger.LogInformation("Approvals result for learning {LearningKey} (apprenticeship {ApprenticeshipId}): all changes auto-approved = {AllAutoApproved}, {PriceCount} price(s) echoed back",
            body.LearningKey, body.ApprenticeshipId, allAutoApproved, response?.Prices.Count ?? 0);
    }

    private static bool IsSuccess(HttpStatusCode statusCode) => (int)statusCode is >= 200 and < 300;

    private static bool IsTransient(HttpStatusCode statusCode) =>
        statusCode is HttpStatusCode.RequestTimeout or HttpStatusCode.TooManyRequests || (int)statusCode >= 500;
}
