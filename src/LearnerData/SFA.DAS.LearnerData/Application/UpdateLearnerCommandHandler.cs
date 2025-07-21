using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class UpdateLearnerCommand : IRequest
{
    public Guid LearningKey { get; set; }
    public UpdateLearnerRequest UpdateLearnerRequest { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class UpdateLearnerCommandHandler(
    ILogger<UpdateLearnerCommandHandler> logger,
    IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> learningApiClient,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient
    ) : IRequestHandler<UpdateLearnerCommand>
{
    public async Task Handle(UpdateLearnerCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating learner with key {LearningKey}", command.LearningKey);
        var request = CreateUpdateLearnerApiPutRequest(command.LearningKey, command);

        var learningResponse = await learningApiClient.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(request);

        if (!learningResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to update learner with key {LearningKey}. Status code: {StatusCode}",
                command.LearningKey, learningResponse.StatusCode);
            throw new Exception($"Failed to update learner with key {command.LearningKey}. Status code: {learningResponse.StatusCode}.");
        }

        var changes = learningResponse.Body;
        if (!changes.Any())
        {
            logger.LogInformation("No changes detected for learner with key {LearningKey}", command.LearningKey);
            return;
        }

        logger.LogInformation("Learner with key {LearningKey} updated successfully. Changes: {@Changes}",
            command.LearningKey, string.Join(", ", changes));

        await UpdateEarnings(command, changes);

        logger.LogInformation("Earnings updated for learner with key {LearningKey}", command.LearningKey);
    }

    private async Task UpdateEarnings(UpdateLearnerCommand command, List<LearningUpdateChanges> learningUpdateChanges)
    {
        foreach (var change in learningUpdateChanges)
        {
            switch (change)
            {
                case LearningUpdateChanges.CompletionDate:
                    logger.LogInformation("Updating earnings for learner with key {LearnerKey} due to completion date change", command.LearningKey);
                    var request = new SaveCompletionApiPutRequest(command.LearningKey, new SaveCompletionRequest
                    {
                        CompletionDate = command.UpdateLearnerRequest.Delivery.CompletionDate
                    });
                    await earningsApiClient.Patch(request);
                    break;
            }
        }
    }

    private static UpdateLearningApiPutRequest CreateUpdateLearnerApiPutRequest(Guid learnerKey, UpdateLearnerCommand command)
    {
        var body = new UpdateLearningRequestBody
        {
            Learner = new LearningUpdateDetails
            {
                CompletionDate = command.UpdateLearnerRequest.Delivery.CompletionDate
            }
        };

        return new UpdateLearningApiPutRequest(learnerKey, body);
    }
}
