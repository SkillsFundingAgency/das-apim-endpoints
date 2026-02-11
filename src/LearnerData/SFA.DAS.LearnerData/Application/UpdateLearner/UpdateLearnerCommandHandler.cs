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

public class UpdateLearnerCommandHandler : IRequestHandler<UpdateLearnerCommand>
{
    private readonly ILogger<UpdateLearnerCommandHandler> _logger;
    private readonly ILearningApiClient<LearningApiConfiguration> _learningApiClient;
    private readonly IEarningsApiClient<EarningsApiConfiguration> _earningsApiClient;
    private readonly IUpdateLearningPutRequestBuilder _updateLearningPutRequestBuilder;
    private readonly IUpdateEarningsOnProgrammeRequestBuilder _updateEarningsOnProgrammeRequestBuilder;
    private readonly IUpdateEarningsEnglishAndMathsRequestBuilder _updateEarningsEnglishAndMathsRequestBuilder;
    private readonly IUpdateEarningsLearningSupportRequestBuilder _updateEarningsLearningSupportRequestBuilder;
    private readonly IDistributedCache _distributedCache;

    public UpdateLearnerCommandHandler(ILogger<UpdateLearnerCommandHandler> logger,
        ILearningApiClient<LearningApiConfiguration> learningApiClient,
        IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
        IUpdateLearningPutRequestBuilder updateLearningPutRequestBuilder,
        IUpdateEarningsOnProgrammeRequestBuilder updateEarningsOnProgrammeRequestBuilder,
        IUpdateEarningsEnglishAndMathsRequestBuilder updateEarningsEnglishAndMathsRequestBuilder,
        IUpdateEarningsLearningSupportRequestBuilder updateEarningsLearningSupportRequestBuilder,
        IDistributedCache distributedCache)
    {
        _logger = logger;

        _logger.LogInformation("UpdateLearnerCommandHandler ctor");

        _learningApiClient = learningApiClient;
        _earningsApiClient = earningsApiClient;
        _updateLearningPutRequestBuilder = updateLearningPutRequestBuilder;
        _updateEarningsOnProgrammeRequestBuilder = updateEarningsOnProgrammeRequestBuilder;
        _updateEarningsEnglishAndMathsRequestBuilder = updateEarningsEnglishAndMathsRequestBuilder;
        _updateEarningsLearningSupportRequestBuilder = updateEarningsLearningSupportRequestBuilder;
        _distributedCache = distributedCache;

        _logger.LogInformation($"Distributed cache: {_distributedCache.GetType().FullName}");
    }

    public async Task Handle(UpdateLearnerCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating learner with key {LearningKey}", command.LearningKey);

        await CacheLearnerData(command, cancellationToken);

        var request = _updateLearningPutRequestBuilder.Build(command);

        var learningResponse = await _learningApiClient.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(request);

        if (!learningResponse.StatusCode.IsSuccessStatusCode())
        {
            _logger.LogError("Failed to update learner with key {LearningKey}. Status code: {StatusCode}",
                command.LearningKey, learningResponse.StatusCode);
            throw new Exception($"Failed to update learner with key {command.LearningKey}. Status code: {learningResponse.StatusCode}.");
        }

        var learningApiPutResponse = learningResponse.Body;

        _logger.LogInformation("Learner with key {LearningKey} updated successfully. Changes: {@Changes}",
            command.LearningKey, string.Join(", ", learningApiPutResponse));
        
        if (learningApiPutResponse.Changes.Count == 0 || learningApiPutResponse.Changes.HasPersonalDetailsOnly())
        {
            _logger.LogInformation("No changes requiring earnings update for learning {LearningKey}", command.LearningKey);
            return;
        }
        
        //Update Earnings
        if (learningApiPutResponse.Changes.HasOnProgrammeUpdate())
        {
            _logger.LogInformation("Updating Earnings with OnProgramme changes for learning {LearningKey}", command.LearningKey);
            var earningsOnProgrammeApiRequest = await _updateEarningsOnProgrammeRequestBuilder.Build(command, learningApiPutResponse, request);
            await _earningsApiClient.Put(earningsOnProgrammeApiRequest);
        }

        if (learningApiPutResponse.Changes.HasEnglishAndMathsUpdate())
        {
            _logger.LogInformation("Updating Earnings with English and Maths changes for learning {LearningKey}", command.LearningKey);
            var englishAndMathsRequest = _updateEarningsEnglishAndMathsRequestBuilder.Build(command, learningApiPutResponse, request);
            await _earningsApiClient.Put(englishAndMathsRequest);
        }

        if (learningApiPutResponse.Changes.HasLearningSupportUpdate())
        {
            _logger.LogInformation("Updating Earnings with Learning Support changes for learning {LearningKey}", command.LearningKey);
            var earningsLearningSupportRequest = _updateEarningsLearningSupportRequestBuilder.Build(command, learningApiPutResponse, request);
            await _earningsApiClient.Put(earningsLearningSupportRequest);
        }

        _logger.LogInformation("Earnings updated for learning {LearningKey}", command.LearningKey);
    }

    private async Task CacheLearnerData(UpdateLearnerCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Caching learner data for learning key {LearningKey} and UKPRN {Ukprn} using {cacheType}", command.LearningKey, command.Ukprn, _distributedCache.GetType().FullName);
        await _distributedCache.StoreLearner(command.UpdateLearnerRequest, command.Ukprn, _logger, cancellationToken);
    }
}