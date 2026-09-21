using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinanceJobs.Domain.Enums;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Collections.Concurrent;
using System.Net;
namespace SFA.DAS.EmployerFinanceJobs.Commands.ImportCommittedLearners;

public class ImportCommittedLearnersCommandHandler(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient,
        IFundingProjectionApiClient<FundingProjectionApiConfiguration> fundingProjectionApiClient,
        ILogger<ImportCommittedLearnersCommandHandler> logger)
        : IRequestHandler<ImportCommittedLearnersCommand, ImportCommittedLearnersCommandResult>
{
    private const int MaxDegreeOfParallelism = 5;
    
    public async Task<ImportCommittedLearnersCommandResult> Handle(
        ImportCommittedLearnersCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Starting import of committed learners");

            // Get or create job state and cutoff date
            var jobState = await GetOrCreateJobStateAsync();
            var cutOffDateTime = jobState.LastSuccessfulImportDate;

            logger.LogInformation("Importing learners since {CutOffDateTime}", cutOffDateTime);

            // Fetch first batch to determine total batches
            var firstBatch = await FetchBatchAsync(cutOffDateTime, batchNumber: 1);

            if (firstBatch == null || !firstBatch.Learners.Any())
            {
                logger.LogInformation("No learners found for import");
                return CreateResult(successfulImports: 0, failedImports: 0, batchesProcessed: 0);
            }

            // Process all batches
            var importResult = await ProcessAllBatchesAsync(
                firstBatch,
                cutOffDateTime,
                cancellationToken);

            // Update job state with results
            await UpdateJobStateAsync(jobState, importResult);

            logger.LogInformation(
                "Import completed. Successful: {Successful}, Failed: {Failed}, Batches: {Batches}",
                importResult.Successful, importResult.Failed, importResult.BatchesProcessed);

            return CreateResult(
                importResult.Successful,
                importResult.Failed,
                importResult.BatchesProcessed);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error during committed learners import");
            throw;
        }
    }

    /// <summary>
    /// Get or create job state for tracking import progress
    /// </summary>
    private async Task<ImportJobStateApiResponse> GetOrCreateJobStateAsync()
    {
        var jobStateResponse = await fundingProjectionApiClient.PostWithResponseCode<ImportJobStateApiResponse>(
            new GetOrCreateJobApiRequest(JobName.ImportLearnerCost));

        if (jobStateResponse.StatusCode is not HttpStatusCode.OK and not HttpStatusCode.Created)
        {
            logger.LogError("Failed to get or create job state. Status: {StatusCode}", jobStateResponse.StatusCode);
            throw new InvalidOperationException(
                $"Failed to get or create job state for {JobName.ImportLearnerCost}. Status: {jobStateResponse.StatusCode}");
        }

        return jobStateResponse.Body;
    }

    /// <summary>
    /// Fetch a single batch of learners
    /// </summary>
    private async Task<GetAllLearnersApiResponse> FetchBatchAsync(
        DateTime cutOffDateTime,
        int batchNumber)
    {
        try
        {
            return await commitmentsV2ApiClient.Get<GetAllLearnersApiResponse>(new GetAllLearnersApiRequest(cutOffDateTime, BatchSize: 1000, BatchNumber: batchNumber));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching batch {BatchNumber}", batchNumber);
            throw;
        }
    }

    /// <summary>
    /// Process all batches sequentially, starting with the first batch
    /// </summary>
    private async Task<(int Successful, int Failed, int BatchesProcessed)> ProcessAllBatchesAsync(
        GetAllLearnersApiResponse firstBatch,
        DateTime cutOffDateTime,
        CancellationToken cancellationToken)
    {
        int successfulImports = 0;
        int failedImports = 0;
        int totalBatches = firstBatch.TotalNumberOfBatches;

        logger.LogInformation("Total batches to process: {TotalBatches}", totalBatches);

        // Process first batch
        var firstBatchResult = await ProcessBatchAsync(firstBatch, cancellationToken);
        successfulImports += firstBatchResult.Successful;
        failedImports += firstBatchResult.Failed;

        // Process remaining batches
        for (int batchNumber = 2; batchNumber <= totalBatches; batchNumber++)
        {
            try
            {
                var batch = await FetchBatchAsync(cutOffDateTime, batchNumber);

                if (batch?.Learners == null || !batch.Learners.Any())
                {
                    logger.LogWarning("Batch {BatchNumber} returned no learners", batchNumber);
                    continue;
                }

                logger.LogDebug("Processing batch {BatchNumber}/{TotalBatches} with {LearnerCount} learners",
                    batchNumber, totalBatches, batch.Learners.Count);

                var batchResult = await ProcessBatchAsync(batch, cancellationToken);
                successfulImports += batchResult.Successful;
                failedImports += batchResult.Failed;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing batch {BatchNumber}", batchNumber);
                failedImports += 1000;  // Estimate based on typical batch size
            }
        }

        return (successfulImports, failedImports, totalBatches);
    }

    /// <summary>
    /// Process a single batch of learners with parallel import
    /// </summary>
    private async Task<(int Successful, int Failed)> ProcessBatchAsync(
        GetAllLearnersApiResponse batch,
        CancellationToken cancellationToken)
    {
        var learnersToImport = batch.Learners
            .Where(lr => lr.PaymentStatus is (int)PaymentStatus.Active or (int)PaymentStatus.Completed)
            .ToList();

        if (!learnersToImport.Any())
        {
            return (0, 0);
        }

        var results = new ConcurrentBag<bool>();

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = MaxDegreeOfParallelism,
            CancellationToken = cancellationToken
        };

        await Parallel.ForEachAsync(learnersToImport, parallelOptions, async (learner, ct) =>
        {
            results.Add(await ImportLearnerAsync(learner));
        });

        int successfulImports = results.Count(r => r);
        int failedImports = results.Count(r => !r);

        logger.LogDebug("Batch result - Successful: {Successful}, Failed: {Failed}",
            successfulImports, failedImports);

        return (successfulImports, failedImports);
    }

    /// <summary>
    /// Import a single learner and return success status
    /// </summary>
    private async Task<bool> ImportLearnerAsync(
        Learner learner)
    {
        try
        {
            var importResponse = await fundingProjectionApiClient.PostWithResponseCode<GetCommittedLearnerApiResponse>(
                new PostCommittedLearnerApiRequest(
                    learner.EmployerAccountId,
                    new PostCommittedLearnerApiRequestData
                    {
                        ApprenticeshipId = learner.ApprenticeshipId,
                        StartDate = learner.StartDate,
                        EndDate = learner.EndDate,
                        CreatedOn = learner.CreatedOn,
                        UpdatedOn = learner.UpdatedOn,
                        PaymentStatus = (PaymentStatus)learner.PaymentStatus
                    }));

            return importResponse.StatusCode is HttpStatusCode.OK or HttpStatusCode.Created;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error importing learner {ApprenticeshipId}", learner.ApprenticeshipId);
            return false;
        }
    }

    /// <summary>
    /// Update job state with import results
    /// </summary>
    private async Task UpdateJobStateAsync(
        ImportJobStateApiResponse jobState,
        (int Successful, int Failed, int BatchesProcessed) importResult)
    {
        try
        {
            bool allSuccessful = importResult.Failed == 0;

            await fundingProjectionApiClient.Put(
                new PutUpdateJobStateApiRequest(
                    jobState.Id,
                    new PutImportJobStateRequestData
                    {
                        LastSuccessfulImportDate = allSuccessful
                            ? DateTime.UtcNow
                            : jobState.LastSuccessfulImportDate,
                        LastAttemptedDate = DateTime.UtcNow,
                        LastAttemptSuccessful = allSuccessful,
                        TotalRecordsLastRun = importResult.Successful + importResult.Failed,
                        FailedRecordsLastRun = importResult.Failed
                    }));

            logger.LogInformation("Job state updated. Success: {Success}", allSuccessful);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update job state");
            throw;
        }
    }

    /// <summary>
    /// Create result from import metrics
    /// </summary>
    private static ImportCommittedLearnersCommandResult CreateResult(
        int successfulImports,
        int failedImports,
        int batchesProcessed)
    {
        return new ImportCommittedLearnersCommandResult
        {
            TotalRecords = successfulImports + failedImports,
            FailedRecords = failedImports,
            BatchesProcessed = batchesProcessed
        };
    }
}