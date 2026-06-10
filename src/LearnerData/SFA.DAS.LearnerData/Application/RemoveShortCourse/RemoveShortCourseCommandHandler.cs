using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Application.Requests.Earnings;
using SFA.DAS.LearnerData.Application.Requests.Learning;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using System.Net;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Application.RemoveShortCourse;

public class RemoveShortCourseCommandHandler(
    ILogger<RemoveShortCourseCommandHandler> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient

) : IRequestHandler<RemoveShortCourseCommand>
{
    public async Task Handle(RemoveShortCourseCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling DeleteShortCourseCommand for Ukprn: {Ukprn}, LearningKey: {LearningKey}", command.Ukprn, command.LearningKey);

        var learningRequest = new DeleteShortCourseApiDeleteRequest(command.Ukprn, command.LearningKey);

        var learningResponse = await learningApiClient.DeleteWithResponseCode<DeleteShortCourseResponse>(learningRequest, true);

        if (!learningResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to delete short course with key {LearningKey}. Status code: {StatusCode}", command.LearningKey, learningResponse.StatusCode);
            throw new Exception($"Failed to delete short course with key {command.LearningKey}. Status code: {learningResponse.StatusCode}.");
        }

        var earningsRequest = new DeleteShortCourseEarningsRequest(command.LearningKey, learningResponse.Body.RemovedEpisodeKey);

        var earningsResponse = await earningsApiClient.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(earningsRequest, true);

        if (!earningsResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to delete short course earnings with key {LearningKey}. Status code: {StatusCode}", command.LearningKey, earningsResponse.StatusCode);
            throw new Exception($"Failed to delete short course earnings with key {command.LearningKey}. Status code: {earningsResponse.StatusCode}.");
        }

        logger.LogInformation("Short course with key {LearningKey} deleted from Learning and Earnings successfully", command.LearningKey);
    }
}
