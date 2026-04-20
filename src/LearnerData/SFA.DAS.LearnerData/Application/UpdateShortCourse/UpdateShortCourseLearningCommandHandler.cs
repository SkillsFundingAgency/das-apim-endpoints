using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using LearningDomainMilestones = SFA.DAS.LearnerData.Requests.LearningInner.Milestone;
using SourceMilestone = SFA.DAS.LearnerData.Requests.Milestone;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Application.UpdateShortCourse;

public class UpdateShortCourseLearningCommandHandler : IRequestHandler<UpdateShortCourseLearningCommand>
{
    private readonly ILogger<UpdateShortCourseLearningCommandHandler> _logger;
    private readonly ILearningApiClient<LearningApiConfiguration> _learningApiClient;
    private readonly IEarningsApiClient<EarningsApiConfiguration> _earningsApiClient;
    private readonly ICalculateGrowthAndSkillsPaymentsEventBuilder _calculateGrowthAndSkillsPaymentsEventBuilder;
    private readonly IMessageSession _messageSession;
    private readonly PaymentsConfiguration _paymentsConfiguration;

    public UpdateShortCourseLearningCommandHandler(
        ILogger<UpdateShortCourseLearningCommandHandler> logger,
        ILearningApiClient<LearningApiConfiguration> learningApiClient,
        IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
        ICalculateGrowthAndSkillsPaymentsEventBuilder calculateGrowthAndSkillsPaymentsEventBuilder,
        IMessageSession messageSession,
        PaymentsConfiguration paymentsConfiguration)
    {
        _logger = logger;
        _learningApiClient = learningApiClient;
        _earningsApiClient = earningsApiClient;
        _calculateGrowthAndSkillsPaymentsEventBuilder = calculateGrowthAndSkillsPaymentsEventBuilder;
        _messageSession = messageSession;
        _paymentsConfiguration = paymentsConfiguration;
    }

    public async Task Handle(UpdateShortCourseLearningCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateShortCourseLearningCommand for Ukprn: {Ukprn}", command.Ukprn);

        var learningRequest = MapToLearningRequest(command);

        var learningResponse = await _learningApiClient.PutWithResponseCode<UpdateShortCourseLearningRequestBody, UpdateShortCourseLearningPutResponse>(learningRequest);

        if (!learningResponse.StatusCode.IsSuccessStatusCode())
        {
            _logger.LogError("Failed to update shortcourse learning with key {LearningKey}. Status code: {StatusCode}",
                command.LearningKey, learningResponse.StatusCode);
            throw new Exception($"Failed to update shortcourse learning with key {command.LearningKey}. Status code: {learningResponse.StatusCode}.");
        }

        _logger.LogInformation("Shortcourse Learning with key {LearningKey} updated successfully. Changes: {@Changes}",
            command.LearningKey, string.Join(", ", learningResponse.Body.Changes));

        ShortCourseEarningsResponse earningsResponse;

        if (EarningsUpdateRequired(learningResponse.Body))
        {
            var earningRequest = MapToEarningRequest(command);
            var response = await _earningsApiClient.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(earningRequest);
            earningsResponse = response.Body;
        }
        else
        {
            _logger.LogInformation("No changes requiring earnings update for shortcourse learning {LearningKey}", command.LearningKey);
            earningsResponse = await _earningsApiClient.Get<ShortCourseEarningGetResponse>(new GetShortCourseEarningsRequest(command.Ukprn, command.LearningKey));
        }

        await PublishEvent(command.Ukprn, learningResponse.Body, earningsResponse);
    }

    private UpdateShortCourseLearningPutRequest MapToLearningRequest(UpdateShortCourseLearningCommand command)
    {
        if (command.Request.Delivery.OnProgramme.Count > 1)
        {
            _logger.LogWarning("Multiple OnProgramme elements supplied for LearningKey: {LearningKey}. Element with earliest StartDate will be processed; subsequent will be ignored", command.LearningKey);
        }

        var currentOnProgramme = command.Request.Delivery.OnProgramme.MinBy(x=>x.StartDate);

        if (currentOnProgramme == null)
        {
            _logger.LogWarning("No OnProgramme data found for LearningKey: {LearningKey}", command.LearningKey);
            throw new InvalidOperationException($"No OnProgramme data found for LearningKey: {command.LearningKey}");
        }

        var milestones = currentOnProgramme.Milestones.Select(sourceMilestone =>
            Enum.Parse<LearningDomainMilestones>(sourceMilestone.ToString())
        ).ToList();

        if (currentOnProgramme.CompletionDate.HasValue && !currentOnProgramme.Milestones.Contains(SourceMilestone.LearningComplete))
            milestones.Add(LearningDomainMilestones.LearningComplete);

        var body = new UpdateShortCourseLearningRequestBody
        {
            LearnerUpdateDetails = new ShortCourseLearnerUpdateDetails
            {
                LearnerRef = command.Request.Learner.LearnerRef
            },
            OnProgramme = new ShortCourseOnProgrammeUpdateDetails
            {
                Ukprn = command.Ukprn,
                ExpectedEndDate = currentOnProgramme.ExpectedEndDate,
                CompletionDate = currentOnProgramme.CompletionDate,
                WithdrawalDate = currentOnProgramme.WithdrawalDate,
                Milestones = milestones
            }
        };

        return new UpdateShortCourseLearningPutRequest(command.LearningKey, body);
    }

    private UpdateShortCourseOnProgrammeEarningPutRequest MapToEarningRequest(UpdateShortCourseLearningCommand command)
    {
        var currentOnProgramme = command.Request.Delivery.OnProgramme.MaxBy(x => x.StartDate);

        if (currentOnProgramme == null)
        {
            _logger.LogWarning("No OnProgramme data found for LearningKey: {LearningKey}", command.LearningKey);
            throw new InvalidOperationException($"No OnProgramme data found for LearningKey: {command.LearningKey}");
        }

        var milestones = currentOnProgramme.Milestones.Select(sourceMilestone =>
            Enum.Parse<LearningDomainMilestones>(sourceMilestone.ToString())
        ).ToList();

        if (currentOnProgramme.CompletionDate.HasValue && !currentOnProgramme.Milestones.Contains(SourceMilestone.LearningComplete))
            milestones.Add(LearningDomainMilestones.LearningComplete);

        var body = new UpdateShortCourseOnProgrammeRequestBody
        {
            WithdrawalDate = currentOnProgramme.WithdrawalDate,
            CompletionDate = currentOnProgramme.CompletionDate,
            Milestones = milestones
        };

        return new UpdateShortCourseOnProgrammeEarningPutRequest(command.LearningKey, body);
    }

    private async Task PublishEvent(long ukprn, UpdateShortCourseLearningPutResponse learningResponse, ShortCourseEarningsResponse earningsResponse)
    {
        _logger.LogInformation("Sending CalculateGrowthAndSkillsPayments command for LearningKey: {LearningKey}", learningResponse.LearningKey);

        var command = await _calculateGrowthAndSkillsPaymentsEventBuilder.Build(ukprn, learningResponse, earningsResponse);

        var options = new SendOptions();
        options.DoNotEnforceBestPractices();
        options.SetDestination(_paymentsConfiguration.PaymentsEndpoint);
        await _messageSession.Send(command, options);

        _logger.LogInformation("CalculateGrowthAndSkillsPayments command sent for LearningKey: {LearningKey}", learningResponse.LearningKey);

        await _messageSession.Publish(new GrowthAndSkillsPaymentsRecalculatedEvent { Command = command });

        _logger.LogInformation("GrowthAndSkillsPaymentsRecalculatedEvent published for LearningKey: {LearningKey}", learningResponse.LearningKey);
    }

    private static bool EarningsUpdateRequired(UpdateShortCourseLearningPutResponse response)
    {
        var changes = response.GetChangesEnums();

        return changes.Contains(ShortCourseUpdateChanges.WithdrawalDate) ||
            changes.Contains(ShortCourseUpdateChanges.Milestone) ||
            changes.Contains(ShortCourseUpdateChanges.CompletionDate);
    }

}
