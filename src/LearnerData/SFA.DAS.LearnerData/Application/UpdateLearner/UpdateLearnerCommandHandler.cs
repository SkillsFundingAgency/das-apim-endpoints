using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.UpdateLearner;

public class UpdateLearnerCommandHandler(
    ILogger<UpdateLearnerCommandHandler> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
    IUpdateLearningPutRequestBuilder updateLearningPutRequestBuilder,
    IUpdateEarningsOnProgrammeRequestBuilder updateEarningsOnProgrammeRequestBuilder,
    IUpdateEarningsEnglishAndMathsRequestBuilder updateEarningsEnglishAndMathsRequestBuilder,
    IUpdateEarningsLearningSupportRequestBuilder updateEarningsLearningSupportRequestBuilder,
    IDistributedCache distributedCache)
    : IRequestHandler<UpdateLearnerCommand>
{
    public async Task Handle(UpdateLearnerCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating learner with key {LearningKey}", command.LearningKey);

        await CacheLearnerData(command, cancellationToken);

        var request = updateLearningPutRequestBuilder.Build(command);

        var learningResponse = await learningApiClient.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(request);

        if (!learningResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to update learner with key {LearningKey}. Status code: {StatusCode}",
                command.LearningKey, learningResponse.StatusCode);
            throw new Exception($"Failed to update learner with key {command.LearningKey}. Status code: {learningResponse.StatusCode}.");
        }

        var learningApiPutResponse = learningResponse.Body;

        logger.LogInformation("Learner with key {LearningKey} updated successfully. Changes: {@Changes}",
            command.LearningKey, string.Join(", ", learningApiPutResponse));
        
        if (learningApiPutResponse.Changes.Count == 0 || learningApiPutResponse.Changes.HasPersonalDetailsOnly())
        {
            logger.LogInformation("No changes requiring earnings update for learning {LearningKey}", command.LearningKey);
            return;
        }
        
        //Update Earnings
        if (learningApiPutResponse.Changes.HasOnProgrammeUpdate())
        {
            logger.LogInformation("Updating Earnings with OnProgramme changes for learning {LearningKey}", command.LearningKey);
            var earningsOnProgrammeApiRequest = await updateEarningsOnProgrammeRequestBuilder.Build(command, learningApiPutResponse, request);
            await earningsApiClient.Put(earningsOnProgrammeApiRequest);
        }

        if (learningApiPutResponse.Changes.HasEnglishAndMathsUpdate())
        {
            logger.LogInformation("Updating Earnings with English and Maths changes for learning {LearningKey}", command.LearningKey);
            var englishAndMathsRequest = updateEarningsEnglishAndMathsRequestBuilder.Build(command, learningApiPutResponse, request);
            await earningsApiClient.Put(englishAndMathsRequest);
        }

        if (learningApiPutResponse.Changes.HasLearningSupportUpdate())
        {
            logger.LogInformation("Updating Earnings with Learning Support changes for learning {LearningKey}", command.LearningKey);
            var earningsLearningSupportRequest = updateEarningsLearningSupportRequestBuilder.Build(command, learningApiPutResponse, request);
            await earningsApiClient.Put(earningsLearningSupportRequest);
        }

        logger.LogInformation("Earnings updated for learning {LearningKey}", command.LearningKey);
    }

    private async Task CacheLearnerData(UpdateLearnerCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Caching learner data for learning key {LearningKey} and UKPRN {Ukprn} using {cacheType}", command.LearningKey, command.Ukprn, distributedCache.GetType().FullName);
        await distributedCache.StoreLearner(command.UpdateLearnerRequest, command.Ukprn, logger, cancellationToken);
    }
}