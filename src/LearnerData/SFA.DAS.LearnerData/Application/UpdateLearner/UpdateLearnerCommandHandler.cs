using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Application.UpdateLearner;

public class UpdateLearnerCommandHandler(
    ILogger<UpdateLearnerCommandHandler> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
    IUpdateLearningPutRequestBuilder updateLearningPutRequestBuilder,
    IUpdateEarningsOnProgrammeRequestBuilder updateEarningsOnProgrammeRequestBuilder,
    IUpdateEarningsEnglishAndMathsRequestBuilder updateEarningsEnglishAndMathsRequestBuilder,
    IApprovalsService approvalsService,
    ILearnerDataCacheService learnerDataCacheService,
    IMessageSession messageSession,
    IApprovedApprenticeshipExistsChecker approvedApprenticeshipExistsChecker,
    ICourseService courseService,
    ILearnerDataEventMapper learnerDataEventMapper,
    FeatureFlags featureFlags)
    : IRequestHandler<UpdateLearnerCommand>
{
    public async Task Handle(UpdateLearnerCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating learner with key {LearnerKey}", command.LearnerKey);

        await learnerDataCacheService.StoreLearner(command.UpdateLearnerRequest, command.Ukprn, cancellationToken);

        logger.LogInformation("Feature toggle ApprenticeshipUpdateLearner is {ApprenticeshipUpdateLearner}", featureFlags.ApprenticeshipUpdateLearner);
        if (featureFlags.ApprenticeshipUpdateLearner)
        {
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
            else if (learningApiPutResponse.NeedsFurtherApproval
                     && !await approvalsService.RequestApproval(command.Ukprn, command.UpdateLearnerRequest.Learner.Uln, learningApiPutResponse))
            {
                logger.LogInformation("Changes for learning {LearningKey} were not auto-approved by Approvals, so Earnings has not been updated", learningApiPutResponse.LearningKey);
            }
            else
            {
                if (learningApiPutResponse.NeedsFurtherApproval)
                {
                    await ClearFurtherApprovalNeeded(learningApiPutResponse);
                }

                var releaseEarnings = false;
                //Update Earnings
                if (learningApiPutResponse.Changes.HasOnProgrammeUpdate())
                {
                    logger.LogInformation("Updating Earnings with OnProgramme changes for learning {LearningKey}", learningApiPutResponse.LearningKey);
                    var earningsOnProgrammeApiRequest = await updateEarningsOnProgrammeRequestBuilder.Build(command.UpdateLearnerRequest, learningApiPutResponse, request.Data);
                    var earningsOnProgrammeResponse = await earningsApiClient.PutWithResponseCode<UpdateOnProgrammeRequest, UpdateOnProgrammeEarningsApiPutResponse>(earningsOnProgrammeApiRequest);

                    if (earningsOnProgrammeResponse.Body.HasNewEarningsProfileVersionBeenGenerated)
                    {
                        releaseEarnings = true;
                    }
                }

                if (learningApiPutResponse.Changes.HasEnglishAndMathsUpdate())
                {
                    logger.LogInformation("Updating Earnings with English and Maths changes for learning {LearningKey}", learningApiPutResponse.LearningKey);
                    var englishAndMathsRequest = updateEarningsEnglishAndMathsRequestBuilder.Build(command, learningApiPutResponse, request);
                    var englishAndMathsResponse = await earningsApiClient.PutWithResponseCode<UpdateEnglishAndMathsRequest, UpdateEnglishAndMathsEarningsApiPutResponse>(englishAndMathsRequest);
                    if (englishAndMathsResponse.Body.HasNewEarningsProfileVersionBeenGenerated)
                    {
                        releaseEarnings = true;
                    }
                }

                logger.LogInformation("Earnings updated for learning {LearningKey}", learningApiPutResponse.LearningKey);

                if (releaseEarnings)
                {
                    var releaseEarningsRequest = new ReleaseEarningsApiPostRequest(learningApiPutResponse.LearningKey,
                        new ReleaseEarningsRequest
                        {
                            LearnerKey = command.LearnerKey,
                            LearnerRef = command.UpdateLearnerRequest.Learner.LearnerRef
                        });
                    await earningsApiClient.Post(releaseEarningsRequest);

                    logger.LogInformation("Release earnings to payments for learning {LearningKey}", learningApiPutResponse.LearningKey);
                }

            }
        }

        await PublishLearnerDataEventsForUnapprovedApprenticeships(command);
    }

    /// <summary>
    /// Learning flags the episode as FurtherApprovalNeeded when an update needs further approval. Now that Approvals has
    /// auto-approved the change, clear it. A failure is logged and swallowed: Approvals has approved, so Earnings must
    /// still be updated, and throwing would also skip the LearnerDataEvent publishing that follows.
    /// </summary>
    private async Task ClearFurtherApprovalNeeded(BaseLearnerApiPutResponse learningApiPutResponse)
    {
        try
        {
            var response = await learningApiClient.PostWithResponseCode<object>(
                new ClearFurtherApprovalNeededApiPostRequest(learningApiPutResponse.LearningKey, learningApiPutResponse.LearningEpisodeKey, learningApiPutResponse.LearningType),
                false);

            if (!response.StatusCode.IsSuccessStatusCode())
            {
                logger.LogError("Change was auto-approved for learning {LearningKey}, episode {EpisodeKey}, but clearing FurtherApprovalNeeded failed with status {StatusCode}. The flag is still set",
                    learningApiPutResponse.LearningKey, learningApiPutResponse.LearningEpisodeKey, response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Change was auto-approved for learning {LearningKey}, episode {EpisodeKey}, but clearing FurtherApprovalNeeded failed. The flag is still set",
                learningApiPutResponse.LearningKey, learningApiPutResponse.LearningEpisodeKey);
        }
    }

    private async Task PublishLearnerDataEventsForUnapprovedApprenticeships(UpdateLearnerCommand command)
    {
        // Group because a return from a Break in Learning (identified by sharing the same course and agreement id
        // as an earlier OnProgramme item) is not a new apprenticeship, so we don't publish a LearnerDataEvent for it.
        var groups = command.UpdateLearnerRequest.Delivery.OnProgramme
            .GroupBy(x => (x.StandardCode, x.AgreementId));

        foreach (var group in groups)
        {
            var (standardCode, agreementId) = group.Key;
            var earliestOnProgramme = group.OrderBy(x => x.StartDate).First();

            var alreadyApproved = await approvedApprenticeshipExistsChecker.Exists(
                command.Ukprn,
                command.UpdateLearnerRequest.Learner.Uln.ToString(),
                standardCode,
                earliestOnProgramme.StartDate);

            if (alreadyApproved)
            {
                logger.LogInformation(
                    "Approved apprenticeship already exists for learner {LearnerKey}, standard {StandardCode}, agreement {AgreementId} - not publishing LearnerDataEvent",
                    command.LearnerKey, standardCode, agreementId);
                continue;
            }

            logger.LogTrace("Publishing LearnerDataEvent for learner {LearnerKey}, standard {StandardCode}", command.LearnerKey, standardCode);
            var learningType = await GetLearningType(standardCode);
            var evt = learnerDataEventMapper.Build(
                command.Ukprn,
                command.UpdateLearnerRequest.Learner,
                earliestOnProgramme,
                learningType,
                command.CorrelationId,
                command.ReceivedOn,
                command.UpdateLearnerRequest.ConsumerReference);
            await messageSession.Publish(evt);
        }
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