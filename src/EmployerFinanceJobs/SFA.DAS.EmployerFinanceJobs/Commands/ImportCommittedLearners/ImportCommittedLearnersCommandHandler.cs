using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinanceJobs.Domain.Enums;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
namespace SFA.DAS.EmployerFinanceJobs.Commands.ImportCommittedLearners;

public class ImportCommittedLearnersCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient,
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> fundingProjectionApiClient,
    ILogger<ImportCommittedLearnersCommandHandler> logger) : IRequestHandler<ImportCommittedLearnersCommand, ImportCommittedLearnersCommandResult>
{
    private const int MaxDegreeOfParallelism = 5;  // Limit concurrent requests to avoid throttling

    public async Task<ImportCommittedLearnersCommandResult> Handle(
            ImportCommittedLearnersCommand request,
            CancellationToken cancellationToken)
    {
        int successfulImports = 0;
        int failedImports = 0;

        try
        {
            logger.LogInformation("Starting import of committed learners since {CutOffDateTime}", request.CutOffDateTime);

            // Fetch first batch to get total number of batches
            var firstBatchResponse = await commitmentsV2ApiClient.Get<GetAllLearnersApiResponse>(
                new GetAllLearnersApiRequest(request.CutOffDateTime));

            if (firstBatchResponse?.Learners == null || !firstBatchResponse.Learners.Any())
            {
                logger.LogInformation("No learners found for import");
                return new ImportCommittedLearnersCommandResult { TotalRecords = 0, FailedRecords = 0 };
            }

            var totalBatches = firstBatchResponse.TotalNumberOfBatches;
            logger.LogInformation("Total batches to process: {TotalBatches}", totalBatches);

            // Process first batch
            var firstBatchResult = await ProcessBatchAsync(firstBatchResponse, cancellationToken);
            successfulImports += firstBatchResult.Successful;
            failedImports += firstBatchResult.Failed;

            // Process remaining batches
            for (int batchNumber = 2; batchNumber <= totalBatches; batchNumber++)
            {
                try
                {
                    var batchResponse = await commitmentsV2ApiClient.Get<GetAllLearnersApiResponse>(new GetAllLearnersApiRequest(request.CutOffDateTime, BatchSize: 1000, BatchNumber: batchNumber));

                    if (batchResponse?.Learners == null || !batchResponse.Learners.Any())
                    {
                        logger.LogWarning("Batch {BatchNumber} returned no learners", batchNumber);
                        continue;
                    }

                    logger.LogInformation("Processing batch {BatchNumber}/{TotalBatches} with {LearnerCount} learners",
                        batchNumber, totalBatches, batchResponse.Learners.Count);

                    var batchResult = await ProcessBatchAsync(batchResponse, cancellationToken);
                    successfulImports += batchResult.Successful;
                    failedImports += batchResult.Failed;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing batch {BatchNumber}", batchNumber);
                    failedImports += 1000;  // Estimate; adjust as needed
                }
            }

            logger.LogInformation("Import completed. Successful: {Successful}, Failed: {Failed}",
                successfulImports, failedImports);

            return new ImportCommittedLearnersCommandResult
            {
                TotalRecords = successfulImports,
                FailedRecords = failedImports,
                BatchesProcessed = totalBatches
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error during committed learners import");
            throw;
        }
    }

    /// <summary>
    /// Process a single batch of learners with parallel import
    /// </summary>
    private async Task<(int Successful, int Failed)> ProcessBatchAsync(
        GetAllLearnersApiResponse batch,
        CancellationToken cancellationToken)
    {
        int successfulImports = 0;
        int failedImports = 0;

        // Filter active and completed learners only
        var learnersToImport = batch.Learners
            .Where(lr => lr.PaymentStatus is (int)PaymentStatus.Active or (int)PaymentStatus.Completed)
            .ToList();

        if (!learnersToImport.Any())
        {
            return (0, 0);
        }

        // Process learners in parallel with concurrency limit
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = MaxDegreeOfParallelism,
            CancellationToken = cancellationToken
        };

        var results = new System.Collections.Concurrent.ConcurrentBag<bool>();

        await Parallel.ForEachAsync(learnersToImport, options, async (learner, ct) =>
        {
            try
            {
                var importResponse = await fundingProjectionApiClient.PostWithResponseCode<GetCommittedLearnerApiResponse>(
                    new PostCommittedLearnerApiRequest(learner.EmployerAccountId, new PostCommittedLearnerApiRequestData
                    {
                        ApprenticeshipId = learner.ApprenticeshipId,
                        StartDate = Convert.ToDateTime(learner.StartDate),
                        EndDate = learner.EndDate,
                        CreatedOn = learner.CreatedOn,
                        UpdatedOn = learner.UpdatedOn,
                        PaymentStatus = (PaymentStatus)learner.PaymentStatus
                    }));

                results.Add(importResponse.StatusCode is HttpStatusCode.OK or HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error importing learner {ApprenticeshipId}", learner.ApprenticeshipId);
                results.Add(false);
            }
        });

        successfulImports = results.Count(r => r);
        failedImports = results.Count(r => !r);

        return (successfulImports, failedImports);
    }
}