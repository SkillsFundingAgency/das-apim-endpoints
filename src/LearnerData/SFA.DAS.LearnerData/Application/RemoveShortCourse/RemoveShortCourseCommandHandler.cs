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
        logger.LogInformation("Handling DeleteShortCourseCommand for Ukprn: {Ukprn}, LearningKey: {LearningKey}", command.Ukprn, command.LearnerKey);

        var learningRequest = new DeleteShortCourseApiDeleteRequest(command.Ukprn, command.LearnerKey);

        var learningResponse = await learningApiClient.DeleteWithResponseCode<DeleteShortCourseResponse>(learningRequest, true);

        if (!learningResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to delete short course with key {LearningKey}. Status code: {StatusCode}", command.LearnerKey, learningResponse.StatusCode);
            throw new Exception($"Failed to delete short course with key {command.LearnerKey}. Status code: {learningResponse.StatusCode}.");
        }

        var learnerRef = learningResponse.Body.Episodes
            .Where(e => e.Ukprn == command.Ukprn)
            .OrderByDescending(e => e.StartDate)
            .Select(e => e.LearnerRef)
            .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(learnerRef))
        {
            throw new InvalidOperationException($"No episode LearnerRef found for Ukprn {command.Ukprn} and LearnerKey {command.LearnerKey}");
        }

        var earningsRequest = new DeleteShortCourseEarningsRequest(
            learningResponse.Body.LearningKey,
            learningResponse.Body.RemovedEpisodeKey,
            learningResponse.Body.LearnerKey,
            learnerRef);

        var earningsResponse = await earningsApiClient.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(earningsRequest, true);

        if (!earningsResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to delete short course earnings with key {LearningKey}. Status code: {StatusCode}", command.LearnerKey, earningsResponse.StatusCode);
            throw new Exception($"Failed to delete short course earnings with key {command.LearnerKey}. Status code: {earningsResponse.StatusCode}.");
        }

        logger.LogInformation("Short course with key {LearningKey} deleted from Learning and Earnings successfully", command.LearnerKey);
    }
}
