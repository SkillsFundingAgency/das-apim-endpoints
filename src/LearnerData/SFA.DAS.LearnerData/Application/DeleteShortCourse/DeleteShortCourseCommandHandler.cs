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

namespace SFA.DAS.LearnerData.Application.DeleteShortCourse;

public class DeleteShortCourseCommandHandler(
    ILogger<DeleteShortCourseCommandHandler> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
    ICalculateGrowthAndSkillsPaymentsEventBuilder calculateGrowthAndSkillsPaymentsEventBuilder,
    IMessageSession messageSession,
    PaymentsConfiguration paymentsConfiguration

) : IRequestHandler<DeleteShortCourseCommand>
{
    public async Task Handle(DeleteShortCourseCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling DeleteShortCourseCommand for Ukprn: {Ukprn}, LearningKey: {LearningKey}", command.Ukprn, command.LearningKey);

        var learningRequest = new DeleteShortCourseApiDeleteRequest(command.Ukprn, command.LearningKey);

        var learningResponse = await learningApiClient.DeleteWithResponseCode<DeleteShortCourseResponse>(learningRequest, true);

        if (!learningResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to delete short course with key {LearningKey}. Status code: {StatusCode}", command.LearningKey, learningResponse.StatusCode);
            throw new Exception($"Failed to delete short course with key {command.LearningKey}. Status code: {learningResponse.StatusCode}.");
        }

        if (learningResponse.StatusCode == HttpStatusCode.NoContent)
        {
            logger.LogInformation("Short course with key {LearningKey} was a no-op in Learning, skipping Earnings delete", command.LearningKey);
            return;
        }

        var earningsRequest = new DeleteShortCourseEarningsRequest(command.LearningKey);

        var earningsResponse = await earningsApiClient.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(earningsRequest, true);

        if (!earningsResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to delete short course earnings with key {LearningKey}. Status code: {StatusCode}", command.LearningKey, earningsResponse.StatusCode);
            throw new Exception($"Failed to delete short course earnings with key {command.LearningKey}. Status code: {earningsResponse.StatusCode}.");
        }

        await PublishEvent(command.Ukprn, learningResponse.Body, earningsResponse.Body);

        logger.LogInformation("Short course with key {LearningKey} deleted from Learning and Earnings successfully", command.LearningKey);
    }

    private async Task PublishEvent(long ukprn, DeleteShortCourseResponse learningResponse, ShortCourseEarningsResponse earningsResponse)
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
