using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Application.UpdateLearner;

public class UpdateLearnerCommandHandler(
    ILogger<UpdateLearnerCommandHandler> logger,
    IMessageSession messageSession,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
    IUpdateLearningPutRequestBuilder updateLearningPutRequestBuilder,
    IUpdateEarningsOnProgrammeRequestBuilder updateEarningsOnProgrammeRequestBuilder,
    IUpdateEarningsEnglishAndMathsRequestBuilder updateEarningsEnglishAndMathsRequestBuilder,
    IUpdateEarningsLearningSupportRequestBuilder updateEarningsLearningSupportRequestBuilder,
    ILearnerDataCacheService learnerDataCacheService,
    FeatureFlags featureFlags)
    : IRequestHandler<UpdateLearnerCommand>
{
    public async Task Handle(UpdateLearnerCommand command, CancellationToken cancellationToken)
    {
        if (featureFlags.ApprenticeshipUpdateLearner)
        {
            logger.LogInformation("Updating learner with key {LearnerKey}", command.LearnerKey);

            await learnerDataCacheService.StoreLearner(command.UpdateLearnerRequest, command.Ukprn, cancellationToken);

            var request = updateLearningPutRequestBuilder.Build(command.Ukprn, command.UpdateLearnerRequest, command.LearnerKey);

            var learningResponse = await learningApiClient.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(request);

            if (!learningResponse.StatusCode.IsSuccessStatusCode())
            {
                logger.LogError("Failed to update learner with key {LearnerKey}. Status code: {StatusCode}",
                    command.LearnerKey, learningResponse.StatusCode);
                throw new Exception($"Failed to update learner with key {command.LearnerKey}. Status code: {learningResponse.StatusCode}.");
            }

            var learningApiPutResponse = learningResponse.Body;

            logger.LogInformation("Learner with key {LearnerKey} updated successfully. Changes: {@Changes}",
                command.LearnerKey, string.Join(", ", learningApiPutResponse));

            if (learningApiPutResponse.Changes.Count == 0 || learningApiPutResponse.Changes.HasPersonalDetailsOnly())
            {
                logger.LogInformation("No changes requiring earnings update for learner {LearnerKey}", command.LearnerKey);
            }
            else
            {
                //Update Earnings
                if (learningApiPutResponse.Changes.HasOnProgrammeUpdate())
                {
                    logger.LogInformation("Updating Earnings with OnProgramme changes for learning {LearningKey}", learningApiPutResponse.LearningKey);
                    var earningsOnProgrammeApiRequest = await updateEarningsOnProgrammeRequestBuilder.Build(command.UpdateLearnerRequest, learningApiPutResponse, request.Data);
                    await earningsApiClient.Put(earningsOnProgrammeApiRequest);
                }

                if (learningApiPutResponse.Changes.HasEnglishAndMathsUpdate())
                {
                    logger.LogInformation("Updating Earnings with English and Maths changes for learning {LearningKey}", learningApiPutResponse.LearningKey);
                    var englishAndMathsRequest = updateEarningsEnglishAndMathsRequestBuilder.Build(command, learningApiPutResponse, request);
                    await earningsApiClient.Put(englishAndMathsRequest);
                }

                if (learningApiPutResponse.Changes.HasLearningSupportUpdate())
                {
                    logger.LogInformation("Updating Earnings with Learning Support changes for learning {LearningKey}", learningApiPutResponse.LearningKey);
                    var earningsLearningSupportRequest = updateEarningsLearningSupportRequestBuilder.Build(learningApiPutResponse, request);
                    await earningsApiClient.Put(earningsLearningSupportRequest);
                }

                logger.LogInformation("Earnings updated for learning {LearningKey}", learningApiPutResponse.LearningKey);
            }
        }
        else
        {
            logger.LogInformation("Skipping apprenticeship learner update for learner {LearnerKey} as feature flag ApprenticeshipUpdateLearner is disabled", command.LearnerKey);
        }

        logger.LogTrace("Publishing LearnerDataEvents");
        foreach (var onProgramme in command.UpdateLearnerRequest.Delivery.OnProgramme)
        {
            var @event = MapToEvent(command, onProgramme);
            await messageSession.Publish(@event);
        }
    }

    private static LearnerDataEvent MapToEvent(UpdateLearnerCommand command, OnProgrammeRequestDetails onProgramme)
    {
        var cost = onProgramme.Costs.GetCostsOrDefault(onProgramme.StartDate).First();

        return new LearnerDataEvent //todo verify assumptions
        {
            ULN = command.UpdateLearnerRequest.Learner.Uln,
            UKPRN = command.Ukprn,
            FirstName = command.UpdateLearnerRequest.Learner.FirstName,
            LastName = command.UpdateLearnerRequest.Learner.LastName,
            Email = command.UpdateLearnerRequest.Learner.Email,
            DoB = command.UpdateLearnerRequest.Learner.Dob,
            StartDate = onProgramme.StartDate,
            PlannedEndDate = onProgramme.ExpectedEndDate,
            PercentageLearningToBeDelivered = null,
            EpaoPrice = cost.EpaoPrice ?? 0,
            TrainingPrice = cost.TrainingPrice,
            AgreementId = onProgramme.AgreementId ?? string.Empty,
            IsFlexiJob = false,
            StandardCode = onProgramme.StandardCode,
            CorrelationId = command.CorrelationId,
            ReceivedDate = command.ReceivedOn,
            ConsumerReference = null,
            LearningType = LearningType.Apprenticeship
        };
    }
}