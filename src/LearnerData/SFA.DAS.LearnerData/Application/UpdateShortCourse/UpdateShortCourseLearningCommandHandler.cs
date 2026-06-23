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
    private readonly IUpdateShortCourseOnProgrammeEarningPutRequestBuilder _updateShortCourseOnProgrammeEarningPutRequestBuilder;
    private readonly IMessageSession _messageSession;

    public UpdateShortCourseLearningCommandHandler(
        ILogger<UpdateShortCourseLearningCommandHandler> logger,
        ILearningApiClient<LearningApiConfiguration> learningApiClient,
        IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
        IUpdateShortCourseOnProgrammeEarningPutRequestBuilder updateShortCourseOnProgrammeEarningPutRequestBuilder,
        IMessageSession messageSession)
    {
        _logger = logger;
        _learningApiClient = learningApiClient;
        _earningsApiClient = earningsApiClient;
        _updateShortCourseOnProgrammeEarningPutRequestBuilder = updateShortCourseOnProgrammeEarningPutRequestBuilder;
        _messageSession = messageSession;
    }

    public async Task Handle(UpdateShortCourseLearningCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateShortCourseLearningCommand for Ukprn: {Ukprn}", command.Ukprn);

        var learningRequest = MapToLearningRequest(command);

        var learningResponse = await _learningApiClient.PutWithResponseCode<UpdateShortCourseLearningRequestBody, UpdateShortCourseLearningPutResponse>(learningRequest);

        if (!learningResponse.StatusCode.IsSuccessStatusCode())
        {
            _logger.LogError("Failed to update shortcourse learner with key {LearnerKey}. Status code: {StatusCode}",
                command.LearnerKey, learningResponse.StatusCode);
            throw new Exception($"Failed to update shortcourse learner with key {command.LearnerKey}. Status code: {learningResponse.StatusCode}.");
        }

        _logger.LogInformation("Shortcourse learner with key {LearnerKey} updated successfully. Changes: {@Changes}",
            command.LearnerKey, string.Join(", ", learningResponse.Body.Changes));

        ShortCourseEarningsResponse earningsResponse;

        if (EarningsUpdateRequired(learningResponse.Body))
        {
            var currentOnProgramme = command.Request.Delivery.OnProgramme.MaxBy(x => x.StartDate);
            if (currentOnProgramme == null)
            {
                _logger.LogWarning("No OnProgramme data found for LearnerKey: {LearnerKey}", command.LearnerKey);
                throw new InvalidOperationException($"No OnProgramme data found for LearnerKey: {command.LearnerKey}");
            }
            var earningBody = _updateShortCourseOnProgrammeEarningPutRequestBuilder.Build(currentOnProgramme);
            var earningRequest = new UpdateShortCourseOnProgrammeEarningPutRequest(learningResponse.Body.LearningKey, learningResponse.Body.UpdatedEpisodeKey, earningBody);
            var response = await _earningsApiClient.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(earningRequest);
            earningsResponse = response.Body;
        }
        else
        {
            _logger.LogInformation("No changes requiring earnings update for shortcourse learner {LearnerKey}", command.LearnerKey);
            earningsResponse = await _earningsApiClient.Get<ShortCourseEarningGetResponse>(new GetShortCourseEarningsRequest(learningResponse.Body.LearningKey, learningResponse.Body.UpdatedEpisodeKey));
        }
    }

    private UpdateShortCourseLearningPutRequest MapToLearningRequest(UpdateShortCourseLearningCommand command)
    {
        if (command.Request.Delivery.OnProgramme.Count > 1)
        {
            _logger.LogWarning("Multiple OnProgramme elements supplied for LearnerKey: {LearnerKey}. Element with earliest StartDate will be processed; subsequent will be ignored", command.LearnerKey);
        }

        var currentOnProgramme = command.Request.Delivery.OnProgramme.MinBy(x=>x.StartDate);

        if (currentOnProgramme == null)
        {
            _logger.LogWarning("No OnProgramme data found for LearnerKey: {LearnerKey}", command.LearnerKey);
            throw new InvalidOperationException($"No OnProgramme data found for LearnerKey: {command.LearnerKey}");
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
                WithdrawalReasonCode = currentOnProgramme.WithdrawalReasonCode,
                Milestones = milestones
            }
        };

        return new UpdateShortCourseLearningPutRequest(command.LearnerKey, body);
    }

    private static bool EarningsUpdateRequired(UpdateShortCourseLearningPutResponse response)
    {
        var changes = response.GetChangesEnums();

        return changes.Contains(ShortCourseUpdateChanges.WithdrawalDate) ||
            changes.Contains(ShortCourseUpdateChanges.Milestone) ||
            changes.Contains(ShortCourseUpdateChanges.CompletionDate);
    }
}
