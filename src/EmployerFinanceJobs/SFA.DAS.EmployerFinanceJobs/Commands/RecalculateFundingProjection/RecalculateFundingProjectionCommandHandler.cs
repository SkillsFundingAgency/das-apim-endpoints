using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinanceJobs.Domain.Enums;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;

namespace SFA.DAS.EmployerFinanceJobs.Commands.RecalculateFundingProjection;

public class RecalculateFundingProjectionCommandHandler(
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> fundingProjectionApiClient,
    ILogger<RecalculateFundingProjectionCommandHandler> logger)
    : IRequestHandler<RecalculateFundingProjectionCommand, RecalculateFundingProjectionCommandResult>
{
    public async Task<RecalculateFundingProjectionCommandResult> Handle(
        RecalculateFundingProjectionCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting recalculation of funding projection");

        var jobState = await GetOrCreateJobStateAsync();

        var recalculationResponse = await fundingProjectionApiClient
            .PostWithResponseCode<RecalculateFundingProjectionApiResponse>(
                new RecalculateFundingProjectionApiRequest(jobState.LastSuccessfulImportDate));

        EnsureSuccess(recalculationResponse.StatusCode, "trigger recalculation of funding projection", HttpStatusCode.OK);

        var body = recalculationResponse.Body;

        return new RecalculateFundingProjectionCommandResult(
            TotalRecordsProcessed: body.TotalRecordsProcessed,
            TotalRecordsUpdated: body.TotalRecordsUpdated,
            TotalRecordsInserted: body.TotalRecordsInserted);
    }

    /// <summary>
    /// Get or create job state for tracking import progress
    /// </summary>
    private async Task<ImportJobStateApiResponse> GetOrCreateJobStateAsync()
    {
        var response = await fundingProjectionApiClient
            .PostWithResponseCode<ImportJobStateApiResponse>(
                new GetOrCreateJobApiRequest(JobName.ImportLearners));

        EnsureSuccess(response.StatusCode, $"get or create job state for {JobName.ImportLearners}",
            HttpStatusCode.OK, HttpStatusCode.Created);

        return response.Body;
    }

    /// <summary>
    /// Ensures the API response is successful
    /// </summary>
    private void EnsureSuccess(HttpStatusCode statusCode, string context, params HttpStatusCode[] validCodes)
    {
        if (validCodes.Contains(statusCode)) return;

        logger.LogError("Failed to {Context}. Status: {StatusCode}", context, statusCode);
        throw new InvalidOperationException($"Failed to {context}. Status: {statusCode}");
    }
}