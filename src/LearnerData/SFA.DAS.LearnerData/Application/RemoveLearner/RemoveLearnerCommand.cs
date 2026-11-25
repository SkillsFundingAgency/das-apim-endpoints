using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;

namespace SFA.DAS.LearnerData.Application.RemoveLearner;

public class RemoveLearnerCommand : IRequest
{
    public Guid LearnerKey { get; set; }
    public long Ukprn { get; set; }
    public int AcademicYear { get; set; }
}

public class RemoveLearnerCommandHandler(
    ILogger<RemoveLearnerCommandHandler> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient
) : IRequestHandler<RemoveLearnerCommand>
{
    public async Task Handle(RemoveLearnerCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Removing learner with key {LearnerKey}", command.LearnerKey);

        var removeRequest = new RemoveLearnerApiDeleteRequest(command.LearnerKey, command.Ukprn, command.AcademicYear);

        var response = await learningApiClient.DeleteWithResponseCode<List<RemovedLearningResponse>>(removeRequest, true);

        if (!response.StatusCode.IsSuccessStatusCode())
        {
            throw new Exception($"Failed to remove learner with key {command.LearnerKey}. Status code: {response.StatusCode}.");
        }

        if (response.Body == null)
        {
            throw new Exception($"Failed to remove learner with key {command.LearnerKey}. Learning response body was null.");
        }

        foreach (var removedLearning in response.Body)
        {
            var learningKey = removedLearning.LearningKey;
            var deleteLearningRequest = new DeleteLearningRequest(learningKey);
            var earningsResponse = await earningsApiClient.DeleteWithResponseCode<NullResponse>(deleteLearningRequest);

            if (!earningsResponse.StatusCode.IsSuccessStatusCode())
            {
                throw new Exception($"Failed to withdraw learning from earnings with key {learningKey}. Status code: {earningsResponse.StatusCode}.");
            }

            await ReleaseEarnings(command.LearnerKey, removedLearning);
        }

        logger.LogInformation("Learner with key {LearnerKey} removed and all learnings withdrawn in earnings successfully", command.LearnerKey);
    }

    /// <summary>
    /// Payments are informed that all earnings have been removed for the learning. Learnings without a LearnerRef
    /// (i.e. manually added rather than submitted via the ILR) are never sent to payments, so are skipped.
    /// </summary>
    private async Task ReleaseEarnings(Guid learnerKey, RemovedLearningResponse removedLearning)
    {
        if (string.IsNullOrWhiteSpace(removedLearning.LearnerRef))
        {
            logger.LogInformation("Skipping release of earnings for removed learning {LearningKey} as it has no LearnerRef", removedLearning.LearningKey);
            return;
        }

        var releaseEarningsRequest = new ReleaseEarningsApiPostRequest(removedLearning.LearningKey,
            new ReleaseEarningsRequest
            {
                LearnerKey = learnerKey,
                LearnerRef = removedLearning.LearnerRef,
                ReleaseType = ReleaseType.All
            });
        await earningsApiClient.Post(releaseEarningsRequest);

        logger.LogInformation("Released earnings to payments for removed learning {LearningKey}", removedLearning.LearningKey);
    }
}