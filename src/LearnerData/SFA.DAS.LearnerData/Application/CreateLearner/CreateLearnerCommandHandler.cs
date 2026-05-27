using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.Apim.Shared.Extensions;

namespace SFA.DAS.LearnerData.Application.CreateLearner;

public class CreateLearnerCommandHandler(
    ILogger<CreateLearnerCommandHandler> logger,
    IMessageSession messageSession,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IUpdateLearningPutRequestBuilder updateLearningPutRequestBuilder,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
    IUpdateEarningsOnProgrammeRequestBuilder updateEarningsOnProgrammeRequestBuilder) : IRequestHandler<CreateLearnerCommand>
{
    public async Task Handle(CreateLearnerCommand command, CancellationToken cancellationToken)
    {
        var updateLearnerCommand = new UpdateLearnerCommand
        {
            Ukprn = command.Ukprn,
            UpdateLearnerRequest = command.Request,
            LearningKey = Guid.Empty
        };

        var updateLearningRequest = updateLearningPutRequestBuilder.Build(updateLearnerCommand);

        var putRequest = new CreateDraftLearningApiPutRequest(updateLearningRequest.Data, command.Ukprn, command.Request.Learner.Uln);

        var learningResponse = await learningApiClient.PutWithResponseCode<UpdateLearningRequestBody, CreateDraftLearnerApiPutResponse>(putRequest);

        if (!learningResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to create draft learner. Status code: {StatusCode}", learningResponse.StatusCode);
            throw new Exception($"Failed to create draft learner. Status code: {learningResponse.StatusCode}.");
        }

        if (learningResponse.Body.Changes.Contains(BaseLearnerApiPutResponse.LearningUpdateChanges.Reinstated))
        {
            logger.LogInformation("Reinstating learner with key {LearningKey}", learningResponse.Body.LearningKey);
            var reinstatedCommand = new UpdateLearnerCommand
            {
                Ukprn = command.Ukprn,
                UpdateLearnerRequest = command.Request,
                LearningKey = learningResponse.Body.LearningKey
            };
            var earningsOnProgrammeApiRequest = await updateEarningsOnProgrammeRequestBuilder.Build(reinstatedCommand, learningResponse.Body, updateLearningRequest);
            await earningsApiClient.Put(earningsOnProgrammeApiRequest);
        }

        logger.LogTrace("Publishing LearnerDataEvent");
        var evt = MapToEvent(command);
        await messageSession.Publish(evt);
    }

    private LearnerDataEvent MapToEvent(CreateLearnerCommand command)
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
            LearningType = LearningType.Apprenticeship
        };
    }
}