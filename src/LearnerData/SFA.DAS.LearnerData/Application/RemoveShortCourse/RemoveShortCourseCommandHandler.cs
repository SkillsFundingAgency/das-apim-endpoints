using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.LearnerData.Application.Requests.Earnings;
using SFA.DAS.LearnerData.Application.Requests.Learning;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.NServiceBus;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;

namespace SFA.DAS.LearnerData.Application.RemoveShortCourse;

public class RemoveShortCourseCommandHandler(
    ILogger<RemoveShortCourseCommandHandler> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
    IMessageSession messageSession,
    PaymentsConfiguration paymentsConfiguration

) : IRequestHandler<RemoveShortCourseCommand>
{
    public async Task Handle(RemoveShortCourseCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling DeleteShortCourseCommand for Ukprn: {Ukprn}, LearningKey: {LearningKey}", command.Ukprn, command.LearnerKey);

        var learningRequest = new DeleteShortCourseApiDeleteRequest(command.Ukprn, command.LearnerKey, command.AcademicYear);

        var learningResponse = await learningApiClient.DeleteWithResponseCode<DeleteShortCourseResponse>(learningRequest, true);

        if (!learningResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to delete short course episodes for learner {LearningKey}. Status code: {StatusCode}", command.LearnerKey, learningResponse.StatusCode);
            throw new Exception($"Failed to delete short course episodes for learner {command.LearnerKey}. Status code: {learningResponse.StatusCode}.");
        }

        foreach (var item in learningResponse.Body.Results)
        {
            var learnerRef = GetLearnerRef(item, command.Ukprn);
            var earningsRequest = new DeleteShortCourseEarningsRequest(item.LearningKey, item.RemovedEpisodeKey, command.LearnerKey, learnerRef);

            var earningsResponse = await earningsApiClient.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(earningsRequest, true);

            if (!earningsResponse.StatusCode.IsSuccessStatusCode())
            {
                logger.LogError("Failed to delete short course earnings with key {LearningKey}. Status code: {StatusCode}", item.LearningKey, earningsResponse.StatusCode);
                throw new Exception($"Failed to delete short course earnings with key {item.LearningKey}. Status code: {earningsResponse.StatusCode}.");
            }

        }

        logger.LogInformation("Short courses for learner {LearnerKey} deleted from Learning and Earnings successfully", command.LearnerKey);
    }

    private string GetLearnerRef(DeleteShortCourseItemResponse learningResponse, long ukprn)
    {
        var learnerRef = learningResponse.Episodes
            .Where(e => e.Ukprn == ukprn)
            .OrderByDescending(e => e.StartDate)
            .Select(e => e.LearnerRef)
            .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(learnerRef))
        {
            throw new InvalidOperationException($"No episode LearnerRef found for Ukprn {ukprn} and LearnerKey {learningResponse.LearnerKey}");
        }
        return learnerRef;
    }

}
