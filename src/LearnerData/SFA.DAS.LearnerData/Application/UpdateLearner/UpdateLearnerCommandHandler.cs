﻿using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.UpdateLearner;

public class UpdateLearnerCommandHandler(
    ILogger<UpdateLearnerCommandHandler> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient
    ) : IRequestHandler<UpdateLearnerCommand>
{
    public async Task Handle(UpdateLearnerCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating learner with key {LearningKey}", command.LearningKey);
        var request = CreateUpdateLearnerApiPutRequest(command.LearningKey, command);

        var learningResponse = await learningApiClient.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(request);

        if (!learningResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to update learner with key {LearningKey}. Status code: {StatusCode}",
                command.LearningKey, learningResponse.StatusCode);
            throw new Exception($"Failed to update learner with key {command.LearningKey}. Status code: {learningResponse.StatusCode}.");
        }

        var learningApiPutResponse = learningResponse.Body;
        if (!learningApiPutResponse.Changes.Any())
        {
            logger.LogInformation("No changes detected for learner with key {LearningKey}", command.LearningKey);
            return;
        }

        logger.LogInformation("Learner with key {LearningKey} updated successfully. Changes: {@Changes}",
            command.LearningKey, string.Join(", ", learningApiPutResponse));

        await UpdateEarnings(command, learningApiPutResponse);

        logger.LogInformation("Earnings updated for learner with key {LearningKey}", command.LearningKey);
    }

    private async Task UpdateEarnings(UpdateLearnerCommand command, UpdateLearnerApiPutResponse updateLearningApiPutResponse)
    {
        var updatePrices = false;

        foreach (var change in updateLearningApiPutResponse.Changes)
        {
            switch (change)
            {
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.CompletionDate:
                    await earningsApiClient.UpdateCompletionDate(command, logger);
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.MathsAndEnglish:
                    await earningsApiClient.UpdateMathAndEnglish(command, logger);
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.LearningSupport:
                    await earningsApiClient.UpdateLearningSupport(command, logger);
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.Prices:
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate:
                    updatePrices = true;
                    break;
            }
        }

        if (updatePrices)
        {
            await earningsApiClient.UpdatePrices(command.LearningKey, updateLearningApiPutResponse, logger);
        }
    }

    private static UpdateLearningApiPutRequest CreateUpdateLearnerApiPutRequest(Guid learnerKey, UpdateLearnerCommand command)
    {
        var body = new UpdateLearningRequestBody
        {
            Learner = new LearningUpdateDetails
            {
                FirstName = command.UpdateLearnerRequest.Learner.FirstName,
                LastName = command.UpdateLearnerRequest.Learner.LastName,
                EmailAddress = command.UpdateLearnerRequest.Learner.Email,
                CompletionDate = command.UpdateLearnerRequest.Delivery.OnProgramme.CompletionDate
            },
            OnProgramme = new OnProgrammeDetails
            {
                ExpectedEndDate = command.UpdateLearnerRequest.Delivery.OnProgramme.ExpectedEndDate,
                Costs = command.UpdateLearnerRequest.Delivery.OnProgramme.Costs.Select(x =>  new Cost
                {
                    TrainingPrice = x.TrainingPrice,
                    EpaoPrice = x.EpaoPrice,
                    FromDate = x.FromDate.Value
                }).ToList()
            },
            MathsAndEnglishCourses = command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Select(x =>
                new MathsAndEnglishDetails
                {
                    Amount = x.Amount,
                    CompletionDate = x.CompletionDate,
                    Course = x.Course,
                    PlannedEndDate = x.EndDate,
                    PriorLearningPercentage = x.PriorLearningPercentage,
                    StartDate = x.StartDate,
                    WithdrawalDate = x.WithdrawalDate
                }).ToList(),
            LearningSupport = command.CombinedLearningSupport()
        };

        return new UpdateLearningApiPutRequest(learnerKey, body);
    }
}