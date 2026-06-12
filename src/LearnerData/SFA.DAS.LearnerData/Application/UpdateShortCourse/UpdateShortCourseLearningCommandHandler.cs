using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
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
    private readonly IUpdateShortCourseOnProgrammeEarningPutRequestBuilder _updateShortCourseOnProgrammeEarningPutRequestBuilder;
    private readonly IMessageSession _messageSession;
    private readonly PaymentsConfiguration _paymentsConfiguration;

    public UpdateShortCourseLearningCommandHandler(
        ILogger<UpdateShortCourseLearningCommandHandler> logger,
        ILearningApiClient<LearningApiConfiguration> learningApiClient,
        IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
        ICalculateGrowthAndSkillsPaymentsEventBuilder calculateGrowthAndSkillsPaymentsEventBuilder,
        IUpdateShortCourseOnProgrammeEarningPutRequestBuilder updateShortCourseOnProgrammeEarningPutRequestBuilder,
        IMessageSession messageSession,
        PaymentsConfiguration paymentsConfiguration)
    {
        _logger = logger;
        _learningApiClient = learningApiClient;
        _earningsApiClient = earningsApiClient;
        _calculateGrowthAndSkillsPaymentsEventBuilder = calculateGrowthAndSkillsPaymentsEventBuilder;
        _updateShortCourseOnProgrammeEarningPutRequestBuilder = updateShortCourseOnProgrammeEarningPutRequestBuilder;
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
                command.LearnerKey, learningResponse.StatusCode);
            throw new Exception($"Failed to update shortcourse learning with key {command.LearnerKey}. Status code: {learningResponse.StatusCode}.");
        }

        _logger.LogInformation("Shortcourse Learning with key {LearningKey} updated successfully. Changes: {@Changes}",
            command.LearnerKey, string.Join(", ", learningResponse.Body.Changes));

        ShortCourseEarningsResponse earningsResponse;

        if (EarningsUpdateRequired(learningResponse.Body))
        {
            var currentOnProgramme = command.Request.Delivery.OnProgramme.MaxBy(x => x.StartDate);
            if (currentOnProgramme == null)
            {
                _logger.LogWarning("No OnProgramme data found for LearningKey: {LearningKey}", command.LearnerKey);
                throw new InvalidOperationException($"No OnProgramme data found for LearningKey: {command.LearnerKey}");
            }
            var earningBody = _updateShortCourseOnProgrammeEarningPutRequestBuilder.Build(currentOnProgramme);
            var earningRequest = new UpdateShortCourseOnProgrammeEarningPutRequest(command.LearnerKey, learningResponse.Body.UpdatedEpisodeKey, earningBody);
            var response = await _earningsApiClient.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(earningRequest);
            earningsResponse = response.Body;
        }
        else
        {
            _logger.LogInformation("No changes requiring earnings update for shortcourse learning {LearningKey}", command.LearnerKey);
            earningsResponse = await _earningsApiClient.Get<ShortCourseEarningGetResponse>(new GetShortCourseEarningsRequest(command.LearnerKey, learningResponse.Body.UpdatedEpisodeKey));
        }

        await PublishEvent(command.Ukprn, learningResponse.Body, earningsResponse);
    }

    private UpdateShortCourseLearningPutRequest MapToLearningRequest(UpdateShortCourseLearningCommand command)
    {
        if (command.Request.Delivery.OnProgramme.Count > 1)
        {
            _logger.LogWarning("Multiple OnProgramme elements supplied for LearningKey: {LearningKey}. Element with earliest StartDate will be processed; subsequent will be ignored", command.LearnerKey);
        }

        var currentOnProgramme = command.Request.Delivery.OnProgramme.MinBy(x=>x.StartDate);

        if (currentOnProgramme == null)
        {
            _logger.LogWarning("No OnProgramme data found for LearningKey: {LearningKey}", command.LearnerKey);
            throw new InvalidOperationException($"No OnProgramme data found for LearningKey: {command.LearnerKey}");
        }

        var milestones = currentOnProgramme.Milestones.Select(sourceMilestone =>
            Enum.Parse<Milestone>(sourceMilestone.ToString())
        ).ToList();

        if (currentOnProgramme.CompletionDate.HasValue && !currentOnProgramme.Milestones.Contains(Milestone.LearningComplete))
            milestones.Add(Milestone.LearningComplete);

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

        return new UpdateShortCourseLearningPutRequest(command.LearnerKey, body);
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
