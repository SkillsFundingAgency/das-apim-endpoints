using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Application.CreateLearner;

public class CreateLearnerCommandHandler(
    ILogger<CreateLearnerCommandHandler> logger,
    IMessageSession messageSession,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    ICreateDraftLearningApiPostRequestBuilder createDraftLearningApiPostRequestBuilder,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
    IUpdateEarningsOnProgrammeRequestBuilder updateEarningsOnProgrammeRequestBuilder) : IRequestHandler<CreateLearnerCommand>
{
    public async Task Handle(CreateLearnerCommand command, CancellationToken cancellationToken)
    {
        //var postRequest = createDraftLearningApiPostRequestBuilder.Build(command.Ukprn, command.Request);

        //var learningResponse = await learningApiClient.PostWithResponseCode<CreateDraftLearnerApiPutResponse>(postRequest);

        //if (!learningResponse.StatusCode.IsSuccessStatusCode())
        //{
        //    logger.LogError("Failed to create draft learner. Status code: {StatusCode}", learningResponse.StatusCode);
        //    throw new InvalidOperationException($"Failed to create draft learner. Status code: {learningResponse.StatusCode}.");
        //}

        //if (learningResponse.Body?.Changes != null && learningResponse.Body.Changes.Contains(BaseLearnerApiPutResponse.LearningUpdateChanges.Reinstated))
        //{
        //    logger.LogInformation("Reinstating learner with key {LearningKey}", learningResponse.Body.LearningKey);
        //    var earningsOnProgrammeApiRequest = await updateEarningsOnProgrammeRequestBuilder.Build(learningResponse.Body.LearningKey, command.Request, learningResponse.Body, (UpdateLearningRequestBody)postRequest.Data);
        //    await earningsApiClient.Put(earningsOnProgrammeApiRequest);
        //}

        logger.LogTrace("Publishing LearnerDataEvents");
        foreach (var onProgramme in command.Request.Delivery.OnProgramme)
        {
            var @event = MapToEvent(command, onProgramme);
            await messageSession.Publish(@event);
        }
    }

    private LearnerDataEvent MapToEvent(CreateLearnerCommand command, CreateLearnerRequest.OnProgrammeDetails onProgramme)
    {
        var cost = onProgramme.Costs.GetCostsOrDefault(onProgramme.StartDate).First();

        return new LearnerDataEvent
        {
            ULN = command.Request.Learner.Uln,
            UKPRN = command.Ukprn,
            FirstName = command.Request.Learner.FirstName,
            LastName = command.Request.Learner.LastName,
            Email = command.Request.Learner.Email,
            DoB = command.Request.Learner.Dob!.Value,
            StartDate = onProgramme.StartDate,
            PlannedEndDate = onProgramme.ExpectedEndDate,
            PercentageLearningToBeDelivered = onProgramme.PercentageOfTrainingLeft,
            EpaoPrice = cost.EpaoPrice ?? 0,
            TrainingPrice = cost.TrainingPrice,
            AgreementId = onProgramme.AgreementId!,
            IsFlexiJob = onProgramme.IsFlexiJob!.Value,
            StandardCode = onProgramme.StandardCode,
            CorrelationId = command.CorrelationId,
            ReceivedDate = command.ReceivedOn,
            ConsumerReference = command.Request.ConsumerReference,
            LearningType = LearningType.Apprenticeship
        };
    }
}