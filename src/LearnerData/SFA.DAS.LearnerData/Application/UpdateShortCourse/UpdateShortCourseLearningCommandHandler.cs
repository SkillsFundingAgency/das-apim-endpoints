using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.NServiceBus;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using SourceMilestone = SFA.DAS.LearnerData.Requests.Milestone;

namespace SFA.DAS.LearnerData.Application.UpdateShortCourse;

public class UpdateShortCourseLearningCommandHandler : IRequestHandler<UpdateShortCourseLearningCommand>
{
    private readonly ILogger<UpdateShortCourseLearningCommandHandler> _logger;
    private readonly ILearningApiClient<LearningApiConfiguration> _learningApiClient;
    private readonly IEarningsApiClient<EarningsApiConfiguration> _earningsApiClient;

    public UpdateShortCourseLearningCommandHandler(
        ILogger<UpdateShortCourseLearningCommandHandler> logger,
        ILearningApiClient<LearningApiConfiguration> learningApiClient,
        IEarningsApiClient<EarningsApiConfiguration> earningsApiClient)
    {
        _logger = logger;
        _learningApiClient = learningApiClient;
        _earningsApiClient = earningsApiClient;
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

        if (!learningResponse.Body.Changes.Any())
        {
            _logger.LogInformation("No changes requiring earnings update for shortcourse learning {LearningKey}", command.LearningKey);
            return;
        }

        var earningRequest = MapToEarningRequest(command);
        await _earningsApiClient.Put(earningRequest);
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
            Enum.Parse<Milestone>(sourceMilestone.ToString())
        ).ToList();

        if (currentOnProgramme.CompletionDate.HasValue && !currentOnProgramme.Milestones.Contains(SourceMilestone.LearningComplete))
            milestones.Add(Milestone.LearningComplete);

        var body = new UpdateShortCourseLearningRequestBody
        {
            LearnerUpdateDetails = new ShortCourseLearnerUpdateDetails
            {
                Uln = command.Request.Learner.Uln,
                FirstName = command.Request.Learner.FirstName,
                LastName = command.Request.Learner.LastName,
                DateOfBirth = command.Request.Learner.Dob,
                EmailAddress = command.Request.Learner.Email
            },
            OnProgramme = new ShortCourseOnProgrammeUpdateDetails
            {
                Ukprn = command.Ukprn,
                Price = 1000,
                StartDate = currentOnProgramme.StartDate,
                ExpectedEndDate = currentOnProgramme.ExpectedEndDate,
                CompletionDate = currentOnProgramme.CompletionDate,
                WithdrawalDate = currentOnProgramme.WithdrawalDate,
                CourseCode = currentOnProgramme.CourseCode,
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
            Enum.Parse<Milestone>(sourceMilestone.ToString())
        ).ToList();

        if (currentOnProgramme.CompletionDate.HasValue && !currentOnProgramme.Milestones.Contains(SourceMilestone.LearningComplete))
            milestones.Add(Milestone.LearningComplete);

        var body = new UpdateShortCourseOnProgrammeRequestBody
        {
            WithdrawalDate = currentOnProgramme.WithdrawalDate,
            CompletionDate = currentOnProgramme.CompletionDate,
            Milestones = milestones
        };

        return new UpdateShortCourseOnProgrammeEarningPutRequest(command.LearningKey, body);
    }
}
