using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.Common.Domain.Types;

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
    ILearnerDataEventMapper learnerDataEventMapper,
    FeatureFlags featureFlags) : IRequestHandler<CreateLearnerCommand>
{
    public async Task Handle(CreateLearnerCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling CreateLearnerCommand for Ukprn {Ukprn}", command.Ukprn);
        var learningType = await GetLearningType(command.Request.Delivery.OnProgramme.First().StandardCode);

        logger.LogInformation("Feature toggle ApprenticeshipCreateDraftLearner is {ApprenticeshipCreateDraftLearner}", featureFlags.ApprenticeshipCreateDraftLearner);
        if (featureFlags.ApprenticeshipCreateDraftLearner)
        {
            var postRequest = createDraftLearningApiPostRequestBuilder.Build(command.Ukprn, command.Request, command.AcademicYear, learningType);

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

        logger.LogTrace("Publishing LearnerDataEvent");
        var onProgramme = command.Request.Delivery.OnProgramme.First();
        var evt = learnerDataEventMapper.Build(
            command.Ukprn,
            command.Request.Learner,
            onProgramme,
            learningType,
            command.CorrelationId,
            command.ReceivedOn,
            command.Request.ConsumerReference);
        await messageSession.Publish(evt);
    }

    private async Task<LearningType> GetLearningType(int standardCode)
    {
        var standard = await courseService.GetStandardDetailsById(standardCode.ToString());

        if (standard == null)
        {
            throw new InvalidCourseException($"Courses API could not find standard {standardCode}.");
        }

        if (!Enum.TryParse<LearningType>(standard.ApprenticeshipType, out var learningType))
        {
            throw new InvalidOperationException($"Unrecognised apprenticeship type '{standard.ApprenticeshipType}' for standard {standardCode}.");
        }

        return learningType;
    }
}