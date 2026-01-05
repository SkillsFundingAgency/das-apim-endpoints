using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.RemoveLearner;

public class RemoveLearnerCommand : IRequest
{
    public Guid LearningKey { get; set; }
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
        logger.LogInformation("Removing learner with key {LearningKey}", command.LearningKey);

        var removeRequest = new RemoveLearnerApiDeleteRequest(command.LearningKey, command.Ukprn);

        var response = await learningApiClient.DeleteWithResponseCode<RemoveLearnerResponse>(removeRequest, true);

        if (!response.StatusCode.IsSuccessStatusCode())
        {
            throw new Exception($"Failed to remove learner with key {command.LearningKey}. Status code: {response.StatusCode}.");
        }

        var lastDayOfLearning = response.Body?.LastDayOfLearning;

        if (lastDayOfLearning == null)
        {
            throw new Exception($"LastDayOfLearning returned from learning inner not found. Cannot withdraw from earnings.");
        }

        var deleteLearningRequest = new DeleteLearningRequest(command.LearningKey);
        var earningsResponse = await earningsApiClient.DeleteWithResponseCode<NullResponse>(deleteLearningRequest);

        if (!earningsResponse.StatusCode.IsSuccessStatusCode())
        {
            throw new Exception($"Failed to withdraw learner from earnings with key {command.LearningKey}. Status code: {earningsResponse.StatusCode}.");
        }

        logger.LogInformation("Learner with key {LearningKey} removed and withdrawn in earnings successfully", command.LearningKey);
    }
}