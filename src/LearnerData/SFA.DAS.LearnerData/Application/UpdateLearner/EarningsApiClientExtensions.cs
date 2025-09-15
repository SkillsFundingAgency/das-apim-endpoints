﻿using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.UpdateLearner;

/// <summary>
/// Extensions for the Earnings API client to handle updates related to learners. This is specifically for use with the UpdateLearnerCommandHandler.
/// </summary>
internal static class EarningsApiClientExtensions
{
    internal static async Task UpdateCompletionDate(this IEarningsApiClient<EarningsApiConfiguration> earningsApiClient, UpdateLearnerCommand command, ILogger<UpdateLearnerCommandHandler> logger)
    {
        await LogAndExecute(async () =>
        {
            await earningsApiClient.Patch(new SaveCompletionApiPatchRequest(command.LearningKey, new SaveCompletionRequest
            {
                CompletionDate = command.UpdateLearnerRequest.Delivery.OnProgramme.CompletionDate
            }));
        }, "completion date", logger, command.LearningKey);

    }

    internal static async Task UpdateMathAndEnglish(this IEarningsApiClient<EarningsApiConfiguration> earningsApiClient, UpdateLearnerCommand command, ILogger<UpdateLearnerCommandHandler> logger)
    {
        await LogAndExecute(async () =>
        {
            var data = new SaveMathsAndEnglishRequest();
            data.AddRange(command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Select(x => new MathsAndEnglishRequestDetail
            {
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Course = x.Course,
                Amount = x.Amount,
                WithdrawalDate = x.WithdrawalDate,
                PriorLearningAdjustmentPercentage = x.PriorLearningPercentage,
                ActualEndDate = x.CompletionDate
            }).ToList());

            await earningsApiClient.Patch(new SaveMathsAndEnglishApiPatchRequest(command.LearningKey, data));

        }, "maths and english", logger, command.LearningKey);
    }

    internal static async Task UpdateLearningSupport(this IEarningsApiClient<EarningsApiConfiguration> earningsApiClient, UpdateLearnerCommand command, ILogger<UpdateLearnerCommandHandler> logger)
    {
        await LogAndExecute(async () =>
        {
            var data = new SaveLearningSupportRequest();
            data.AddRange(command.CombinedLearningSupport().Select(ls => new LearningSupportPaymentDetail
            {
                StartDate = ls.StartDate,
                EndDate = ls.EndDate
            }).ToList());

            await earningsApiClient.Patch(new SaveLearningSupportApiPutRequest(command.LearningKey, data));
        }, "learning support", logger, command.LearningKey);
    }

    internal static async Task UpdatePrices(this IEarningsApiClient<EarningsApiConfiguration> earningsApiClient, Guid apprenticeshipKey, UpdateLearnerApiPutResponse apiPutResponse, ILogger<UpdateLearnerCommandHandler> logger)
    {
        await LogAndExecute(async () =>
        {
            var data = new SavePricesRequest
            {
                ApprenticeshipEpisodeKey = apiPutResponse.LearningEpisodeKey,
                Prices = apiPutResponse.Prices.Select(x => new PriceDetail
                {
                    Key = x.Key,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    TrainingPrice = x.TrainingPrice,
                    EndPointAssessmentPrice = x.EndPointAssessmentPrice,
                    TotalPrice = x.TotalPrice,
                    FundingBandMaximum = x.FundingBandMaximum
                }).ToList()
            };

            await earningsApiClient.Patch(new SavePricesApiPatchRequest(apprenticeshipKey, data));
        }, "prices", logger, apprenticeshipKey);
    }

    private static async Task LogAndExecute(Func<Task> action, string updateTarget, ILogger<UpdateLearnerCommandHandler> logger, Guid learningKey)
    {
        logger.LogInformation("Calling Earnings Inner Api to update {updateTarget} for {learningKey}", updateTarget, learningKey);

        try
        {
            await action();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update {updateTarget} for {learningKey}", updateTarget, learningKey);
            throw;
        }

        logger.LogInformation("{updateTarget} updated successfully for {learningKey}", updateTarget, learningKey);
    }
}
