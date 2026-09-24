using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Collections.Concurrent;

namespace SFA.DAS.EmployerFinanceJobs.Commands.UpdateLearnerCost;

public class UpdateLearnerCostCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient,
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> fundingProjectionApiClient,
    ILogger<UpdateLearnerCostCommandHandler> logger) : IRequestHandler<UpdateLearnerCostCommand, UpdateLearnerCostCommandResult>
{
    private const int MaxDegreeOfParallelism = 5;
    private const string ImportStatusPending = "Pending";

    public async Task<UpdateLearnerCostCommandResult> Handle(UpdateLearnerCostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Starting update of learner costs for pending learners");

            // Get pending learners
            var pendingLearners = await fundingProjectionApiClient.Get<List<GetCommittedLearnerApiResponse>>(
                new GetPendingImportLearnersByStatusApiRequest(ImportStatusPending));

            // Validate
            if (pendingLearners == null || !pendingLearners.Any())
            {
                logger.LogInformation("No pending learners found to update");
                return new UpdateLearnerCostCommandResult
                {
                    TotalRecords = 0,
                    SuccessfulRecords = 0,
                    FailedRecords = 0
                };
            }

            logger.LogInformation("Found {PendingLearnerCount} pending learners to update", pendingLearners.Count);

            // Process learners in parallel
            var results = new ConcurrentBag<bool>();

            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken
            };

            await Parallel.ForEachAsync(pendingLearners, options, async (learner, _) =>
            {
                if (!await ProcessLearnerAsync(learner))
                {
                    results.Add(false);
                }
                else
                {
                    results.Add(true);
                }
            });

            int successfulRecords = results.Count(r => r);
            int failedRecords = results.Count(r => !r);

            logger.LogInformation("Learner cost update completed. Successful: {Successful}, Failed: {Failed}",
                successfulRecords, failedRecords);

            return new UpdateLearnerCostCommandResult
            {
                TotalRecords = pendingLearners.Count,
                SuccessfulRecords = successfulRecords,
                FailedRecords = failedRecords
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error during learner cost update");
            throw;
        }
    }


    /// <summary>
    /// Process a single learner's cost update
    /// </summary>
    private async Task<bool> ProcessLearnerAsync(GetCommittedLearnerApiResponse learner)
    {
        try
        {
            logger.LogDebug("Processing learner {ApprenticeshipId}", learner.ApprenticeshipId);

            // Fetch apprenticeship details
            var apprenticeship = await commitmentsV2ApiClient.Get<GetApprenticeshipResponse>(
                new GetApprenticeshipsApiRequest(learner.ApprenticeshipId));

            if (apprenticeship == null)
            {
                logger.LogWarning("Apprenticeship not found for {ApprenticeshipId}", learner.ApprenticeshipId);
                return false;
            }

            if (apprenticeship.PriceReducedBy == 0) // skip learners with zero/no reduced price
            {
                logger.LogInformation("Apprenticeship {ApprenticeshipId} has no reduced price", learner.ApprenticeshipId);
                return true;
            }

            // Update learner with apprenticeship details and cost
            await fundingProjectionApiClient.Put(
                new PutCommittedLearnerApiRequest(
                    apprenticeship.EmployerAccountId,
                    learner.Id,
                    new PutCommittedLearnerApiRequestData
                    {
                        ApprenticeshipId = learner.ApprenticeshipId,
                        StartDate = apprenticeship.StartDate,
                        EndDate = apprenticeship.EndDate,
                        CreatedOn = learner.CreatedOn,
                        UpdatedOn = learner.UpdatedOn,
                        PaymentStatus = learner.PaymentStatus,
                        CommitmentId = apprenticeship.CohortId,
                        Cost = Convert.ToDecimal(apprenticeship.PriceReducedBy ?? 0)
                    }));

            logger.LogInformation("Successfully updated learner {ApprenticeshipId} with cost {Cost}",
                learner.ApprenticeshipId, apprenticeship.PriceReducedBy);

            return true;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "API request failed for learner {ApprenticeshipId}", learner.ApprenticeshipId);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating learner {ApprenticeshipId}", learner.ApprenticeshipId);
            return false;
        }
    }
}