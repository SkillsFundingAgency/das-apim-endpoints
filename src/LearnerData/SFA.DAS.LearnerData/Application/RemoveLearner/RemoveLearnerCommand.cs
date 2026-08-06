using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;

namespace SFA.DAS.LearnerData.Application.RemoveLearner;

public class RemoveLearnerCommand : IRequest
{
    public Guid LearnerKey { get; set; }
    public long Ukprn { get; set; }
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

        var removeRequest = new RemoveLearnerApiDeleteRequest(command.LearnerKey, command.Ukprn);

        var response = await learningApiClient.DeleteWithResponseCode<List<Guid>>(removeRequest, true);

        if (!response.StatusCode.IsSuccessStatusCode())
        {
            throw new Exception($"Failed to remove learner with key {command.LearnerKey}. Status code: {response.StatusCode}.");
        }

        if (response.Body == null)
        {
            throw new Exception($"Failed to remove learner with key {command.LearnerKey}. Learning response body was null.");
        }

        foreach (var learningKey in response.Body)
        {
            var deleteLearningRequest = new DeleteLearningRequest(learningKey);
            var earningsResponse = await earningsApiClient.DeleteWithResponseCode<NullResponse>(deleteLearningRequest);

            if (!earningsResponse.StatusCode.IsSuccessStatusCode())
            {
                throw new Exception($"Failed to withdraw learning from earnings with key {learningKey}. Status code: {earningsResponse.StatusCode}.");
            }
        }

        logger.LogInformation("Learner with key {LearnerKey} removed and all learnings withdrawn in earnings successfully", command.LearnerKey);
    }
}