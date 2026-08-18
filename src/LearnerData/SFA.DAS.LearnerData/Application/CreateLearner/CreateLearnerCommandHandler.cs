using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.LearnerData.Configuration;

namespace SFA.DAS.LearnerData.Application.CreateLearner;

public class CreateLearnerCommandHandler(
    ILogger<CreateLearnerCommandHandler> logger,
    IMessageSession messageSession,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    ICreateDraftLearningApiPostRequestBuilder createDraftLearningApiPostRequestBuilder,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
    IUpdateEarningsOnProgrammeRequestBuilder updateEarningsOnProgrammeRequestBuilder,
    ICreateUnapprovedApprenticeshipLearningRequestBuilder createUnapprovedApprenticeshipLearningRequestBuilder,
    ICourseService courseService,
    FeatureFlags featureFlags) : IRequestHandler<CreateLearnerCommand>
{
    public async Task Handle(CreateLearnerCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling CreateLearnerCommand for Ukprn {Ukprn}", command.Ukprn);
        logger.LogInformation("Feature toggle ApprenticeshipCreateDraftLearner is {ApprenticeshipCreateDraftLearner}", featureFlags.ApprenticeshipCreateDraftLearner);
        if (featureFlags.ApprenticeshipCreateDraftLearner)
        {
            var postRequest = createDraftLearningApiPostRequestBuilder.Build(command.Ukprn, command.Request);

            var learningResponse = await learningApiClient.PostWithResponseCode<CreateDraftLearnerApiPutResponse>(postRequest);

            if (!learningResponse.StatusCode.IsSuccessStatusCode())
            {
                logger.LogError("Failed to create draft learner. Status code: {StatusCode}", learningResponse.StatusCode);
                throw new InvalidOperationException($"Failed to create draft learner. Status code: {learningResponse.StatusCode}.");
            }

            if (featureFlags.ApprenticeshipEarningsGeneration)
            {
                if (learningResponse.Body?.Changes != null && learningResponse.Body.Changes.Contains(BaseLearnerApiPutResponse.LearningUpdateChanges.Reinstated))
                {
                    logger.LogInformation("Reinstating learner with learning key {LearningKey}", learningResponse.Body.LearningKey);
                    var earningsOnProgrammeApiRequest = await updateEarningsOnProgrammeRequestBuilder.Build(learningResponse.Body.LearningKey, command.Request, learningResponse.Body, (UpdateLearningRequestBody)postRequest.Data);
                    await earningsApiClient.Put(earningsOnProgrammeApiRequest);
                }
                else if (learningResponse.Body != null)
                {
                    logger.LogInformation("Creating draft learner with learning key {LearningKey}", learningResponse.Body.LearningKey);
                    var createUnapprovedApprenticeshipLearningRequest = await createUnapprovedApprenticeshipLearningRequestBuilder.Build(command.Ukprn, command.Request, learningResponse.Body, (UpdateLearningRequestBody)postRequest.Data);
                    var earningsResponse = await earningsApiClient.PostWithResponseCode<object>(createUnapprovedApprenticeshipLearningRequest);
                    if (!earningsResponse.StatusCode.IsSuccessStatusCode())
                    {
                        logger.LogError("Failed to create unapproved apprenticeship learning in earnings. Status code: {StatusCode}", earningsResponse.StatusCode);
                        throw new InvalidOperationException($"Failed to create unapproved apprenticeship learning in earnings. Status code: {earningsResponse.StatusCode}.");
                    }
                }

                if (learningResponse.Body?.RemovedLearningKey is { } removedLearningKey)
                {
                    logger.LogInformation("Deleting omitted course's draft earnings with learning key {RemovedLearningKey}", removedLearningKey);
                    var deleteLearningRequest = new DeleteLearningRequest(removedLearningKey);
                    var deleteResponse = await earningsApiClient.DeleteWithResponseCode<NullResponse>(deleteLearningRequest);
                    if (!deleteResponse.StatusCode.IsSuccessStatusCode())
                    {
                        logger.LogError("Failed to delete omitted course's draft earnings. Status code: {StatusCode}", deleteResponse.StatusCode);
                        throw new InvalidOperationException($"Failed to delete omitted course's draft earnings. Status code: {deleteResponse.StatusCode}.");
                    }
                }
            }
        }

        var learningType = await GetLearningType(command.Request.Delivery.OnProgramme.First().StandardCode);

        logger.LogTrace("Publishing LearnerDataEvent");
        var evt = MapToEvent(command, learningType);
        await messageSession.Publish(evt);
    }

    private async Task<LearningType> GetLearningType(int standardCode)
    {
        var standard = await courseService.GetStandardDetailsById(standardCode.ToString());

        if (standard == null)
        {
            throw new InvalidOperationException($"Courses API returned no data for standard {standardCode}.");
        }

        if (!Enum.TryParse<LearningType>(standard.ApprenticeshipType, out var learningType))
        {
            throw new InvalidOperationException($"Unrecognised apprenticeship type '{standard.ApprenticeshipType}' for standard {standardCode}.");
        }

        return learningType;
    }

    private static LearnerDataEvent MapToEvent(CreateLearnerCommand command, LearningType learningType)
    {
        var onProgramme = command.Request.Delivery.OnProgramme.First();
        var cost = onProgramme.Costs.GetCostsOrDefault(onProgramme.StartDate).First();

        return new LearnerDataEvent
        {
            ULN = command.Request.Learner.Uln,
            UKPRN = command.Ukprn,
            FirstName = command.Request.Learner.FirstName,
            LastName = command.Request.Learner.LastName,
            Email = command.Request.Learner.Email,
            DoB = command.Request.Learner.Dob!.Value,
            StartDate = command.Request.Delivery.OnProgramme.First().StartDate,
            PlannedEndDate = command.Request.Delivery.OnProgramme.First().ExpectedEndDate,
            PercentageLearningToBeDelivered = command.Request.Delivery.OnProgramme.First().PercentageOfTrainingLeft,
            EpaoPrice = cost.EpaoPrice ?? 0,
            TrainingPrice = cost.TrainingPrice,
            AgreementId = command.Request.Delivery.OnProgramme.First().AgreementId,
            IsFlexiJob = command.Request.Delivery.OnProgramme.First().IsFlexiJob!.Value,
            StandardCode = command.Request.Delivery.OnProgramme.First().StandardCode,
            CorrelationId = command.CorrelationId,
            ReceivedDate = command.ReceivedOn,
            ConsumerReference = command.Request.ConsumerReference,
            LearningType = learningType
        };
    }
}