using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Application.Requests.Earnings;
using SFA.DAS.LearnerData.Application.Requests.Learning;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;

namespace SFA.DAS.LearnerData.Application.DeleteShortCourse;

public class DeleteShortCourseCommandHandler(
    ILogger<DeleteShortCourseCommandHandler> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient
) : IRequestHandler<DeleteShortCourseCommand>
{
    public async Task Handle(DeleteShortCourseCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling DeleteShortCourseCommand for Ukprn: {Ukprn}, LearningKey: {LearningKey}", command.Ukprn, command.LearningKey);

        var learningRequest = new DeleteShortCourseApiDeleteRequest(command.Ukprn, command.LearningKey);

        var learningResponse = await learningApiClient.DeleteWithResponseCode<NullResponse>(learningRequest);

        if (!learningResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to delete short course with key {LearningKey}. Status code: {StatusCode}", command.LearningKey, learningResponse.StatusCode);
            throw new Exception($"Failed to delete short course with key {command.LearningKey}. Status code: {learningResponse.StatusCode}.");
        }

        if (learningResponse.StatusCode != HttpStatusCode.NoContent)
        {
            logger.LogInformation("Short course with key {LearningKey} was a no-op in Learning, skipping Earnings delete", command.LearningKey);
            return;
        }

        var earningsRequest = new DeleteShortCourseEarningsRequest(command.LearningKey);

        var earningsResponse = await earningsApiClient.DeleteWithResponseCode<NullResponse>(earningsRequest);

        if (!earningsResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to delete short course earnings with key {LearningKey}. Status code: {StatusCode}", command.LearningKey, earningsResponse.StatusCode);
            throw new Exception($"Failed to delete short course earnings with key {command.LearningKey}. Status code: {earningsResponse.StatusCode}.");
        }

        logger.LogInformation("Short course with key {LearningKey} deleted from Learning and Earnings successfully", command.LearningKey);
    }
}
