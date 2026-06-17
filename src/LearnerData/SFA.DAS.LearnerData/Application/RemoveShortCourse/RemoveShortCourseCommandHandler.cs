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
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
    ICalculateGrowthAndSkillsPaymentsEventBuilder calculateGrowthAndSkillsPaymentsEventBuilder,
    IMessageSession messageSession,
    PaymentsConfiguration paymentsConfiguration

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

        foreach (var item in learningResponse.Body.Results)
        {
            var earningsRequest = new DeleteShortCourseEarningsRequest(item.LearningKey, item.RemovedEpisodeKey);

            var earningsResponse = await earningsApiClient.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(earningsRequest, true);

            if (!earningsResponse.StatusCode.IsSuccessStatusCode())
            {
                logger.LogError("Failed to delete short course earnings with key {LearningKey}. Status code: {StatusCode}", item.LearningKey, earningsResponse.StatusCode);
                throw new Exception($"Failed to delete short course earnings with key {item.LearningKey}. Status code: {earningsResponse.StatusCode}.");
            }

            await PublishEvent(command.Ukprn, item, earningsResponse.Body);
        }

        logger.LogInformation("Short course with key {LearnerKey} deleted from Learning and Earnings successfully", command.LearnerKey);
    }

    private async Task PublishEvent(long ukprn, DeleteShortCourseItemResponse learningResponse, ShortCourseEarningsResponse earningsResponse)
    {
        logger.LogInformation("Sending CalculateGrowthAndSkillsPayments command for LearningKey: {LearningKey}", learningResponse.LearningKey);

        var command = await calculateGrowthAndSkillsPaymentsEventBuilder.Build(ukprn, learningResponse, earningsResponse);

        var options = new SendOptions();
        options.DoNotEnforceBestPractices();
        options.SetDestination(paymentsConfiguration.PaymentsEndpoint);
        await messageSession.Send(command, options);

        logger.LogInformation("CalculateGrowthAndSkillsPayments command sent for LearningKey: {LearningKey}", learningResponse.LearningKey);

        await messageSession.Publish(new GrowthAndSkillsPaymentsRecalculatedEvent { Command = command });

        logger.LogInformation("GrowthAndSkillsPaymentsRecalculatedEvent published for LearningKey: {LearningKey}", learningResponse.LearningKey);
    }
}
