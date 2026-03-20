using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.NServiceBus;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

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

        if (!EarningsUpdateRequired(learningResponse.Body))
        {
            _logger.LogInformation("No changes requiring earnings update for shortcourse learning {LearningKey}", command.LearningKey);
            return;
        }

        var earningRequest = MapToEarningRequest(command);
        await _earningsApiClient.Put(earningRequest);

    }

    private UpdateShortCourseLearningPutRequest MapToLearningRequest(UpdateShortCourseLearningCommand command)
    {
        var currentOnProgramme = command.Request.Delivery.OnProgramme.MaxBy(x=>x.StartDate);

        if (currentOnProgramme == null)
        {
            _logger.LogWarning("No OnProgramme data found for LearningKey: {LearningKey}", command.LearningKey);
            throw new InvalidOperationException($"No OnProgramme data found for LearningKey: {command.LearningKey}");
        }

        var milestones = currentOnProgramme.Milestones.Select(sourceMilestone =>
            Enum.Parse<SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses.Milestone>(sourceMilestone.ToString())
        ).ToList();

        var body = new UpdateShortCourseLearningRequestBody
        {
            LearnerUpdateDetails = new ShortCourseLearnerUpdateDetails
            {
                Uln = command.Request.Learner.Uln,
                FirstName = command.Request.Learner.FirstName,
                LastName = command.Request.Learner.LastName,
                DateOfBirth = command.Request.Learner.Dob,
                EmailAddress = command.Request.Learner.Email,
                LearnerRef = command.Request.Learner.LearnerRef
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
            Enum.Parse<SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses.Milestone>(sourceMilestone.ToString())
        ).ToList();

        var body =  new UpdateShortCourseOnProgrammeRequestBody
        {
            WithdrawalDate = currentOnProgramme.WithdrawalDate,
            Milestones = milestones
        };

        return new UpdateShortCourseOnProgrammeEarningPutRequest(command.LearningKey, body);
    }

    private static bool EarningsUpdateRequired(UpdateShortCourseLearningPutResponse response)
    {
        var changes = response.GetChangesEnums();

        return changes.Contains(ShortCourseUpdateChanges.WithdrawalDate) || 
            changes.Contains(ShortCourseUpdateChanges.Milestone) ||
            changes.Contains(ShortCourseUpdateChanges.CompletionDate);
    }
}