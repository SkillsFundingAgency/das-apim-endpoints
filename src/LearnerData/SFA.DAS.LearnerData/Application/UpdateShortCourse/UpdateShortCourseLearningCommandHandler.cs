using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.LearnerData.Application.Requests.Earnings;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.LearnerData.Services.ShortCourses;
using LearningInnerOnProgramme = SFA.DAS.LearnerData.Requests.LearningInner.OnProgramme;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using EarningsOnProgramme = SFA.DAS.LearnerData.Requests.EarningsInner.OnProgramme;
using SharedLearningType = SFA.DAS.SharedOuterApi.Types.Constants.LearningType;

namespace SFA.DAS.LearnerData.Application.UpdateShortCourse;

public class UpdateShortCourseLearningCommandHandler : IRequestHandler<UpdateShortCourseLearningCommand>
{
    private readonly ILogger<UpdateShortCourseLearningCommandHandler> _logger;
    private readonly ILearningApiClient<LearningApiConfiguration> _learningApiClient;
    private readonly IEarningsApiClient<EarningsApiConfiguration> _earningsApiClient;
    private readonly IUpdateShortCourseOnProgrammeEarningPutRequestBuilder _updateShortCourseOnProgrammeEarningPutRequestBuilder;
    private readonly IShortCourseLookupService _shortCourseLookupService;
    private readonly IMessageSession _messageSession;
    private readonly ILearnerDataCacheService _learnerDataCacheService;

    public UpdateShortCourseLearningCommandHandler(
        ILogger<UpdateShortCourseLearningCommandHandler> logger,
        ILearningApiClient<LearningApiConfiguration> learningApiClient,
        IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
        IUpdateShortCourseOnProgrammeEarningPutRequestBuilder updateShortCourseOnProgrammeEarningPutRequestBuilder,
        IShortCourseLookupService shortCourseLookupService,
        IMessageSession messageSession,
        ILearnerDataCacheService learnerDataCacheService)
    {
        _logger = logger;
        _learningApiClient = learningApiClient;
        _earningsApiClient = earningsApiClient;
        _updateShortCourseOnProgrammeEarningPutRequestBuilder = updateShortCourseOnProgrammeEarningPutRequestBuilder;
        _shortCourseLookupService = shortCourseLookupService;
        _messageSession = messageSession;
        _learnerDataCacheService = learnerDataCacheService;
    }

    public async Task Handle(UpdateShortCourseLearningCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateShortCourseLearningCommand for Ukprn: {Ukprn}", command.Ukprn);

        await _learnerDataCacheService.StoreLearner(command.Request, command.Ukprn, cancellationToken);

        var courseDetails = await Task.WhenAll(command.Request.Delivery.OnProgramme
            .Select(onProg => _shortCourseLookupService.GetCourseDetails(onProg.CourseCode, onProg.StartDate)));

        var learningRequest = BuildLearningRequest(command, courseDetails);

        var learningResponse = await _learningApiClient.PutWithResponseCode<UpdateShortCourseLearningRequestBody, UpdateShortCourseLearningResponse>(learningRequest);

        if (!learningResponse.StatusCode.IsSuccessStatusCode())
        {
            _logger.LogError("Failed to update short course learner with key {LearnerKey}. Status: {StatusCode}",
                command.LearnerKey, learningResponse.StatusCode);
            throw new Exception($"Failed to update short course learner {command.LearnerKey}. Status: {learningResponse.StatusCode}.");
        }

        foreach (var (onProg, result, details) in command.Request.Delivery.OnProgramme.Zip(learningResponse.Body.Results, courseDetails))
        {
            if (result.IsIgnored)
            {
                _logger.LogInformation("Ignoring OnProgramme item for CourseCode {CourseCode}", result.CourseCode);
                continue;
            }
            
            if (result.IsNewLearning || result.IsNewEpisode)
            {
                await HandleNewLearning(command, onProg, result, details);
            }
            else
            {
                _logger.LogInformation("Short course learner {LearnerKey} / {CourseCode} updated. Changes: {Changes}",
                    command.LearnerKey, result.CourseCode, string.Join(", ", result.Changes));

                await HandleExistingLearning(command, onProg, result);
            }
        }

        foreach (var removedResult in learningResponse.Body.Results.Where(r => r.IsRemoved))
        {
            await HandleRemovedLearning(command, removedResult);
        }
    }

    private async Task HandleRemovedLearning(UpdateShortCourseLearningCommand command, UpdateShortCourseLearningPutResponse removedResult)
    {
        _logger.LogInformation("Removing omitted Learning {LearningKey} / {CourseCode} from Earnings for LearnerKey {LearnerKey}",
            removedResult.LearningKey, removedResult.CourseCode, command.LearnerKey);

        var earningsRequest = new DeleteShortCourseEarningsRequest(removedResult.LearningKey, removedResult.UpdatedEpisodeKey, command.LearnerKey, command.Request.Learner.LearnerRef);
        var earningsResponse = await _earningsApiClient.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(earningsRequest, true);

        if (!earningsResponse.StatusCode.IsSuccessStatusCode())
        {
            _logger.LogError("Failed to delete earnings for omitted Learning {LearningKey}. Status: {StatusCode}",
                removedResult.LearningKey, earningsResponse.StatusCode);
            throw new Exception($"Failed to delete earnings for omitted Learning {removedResult.LearningKey}. Status: {earningsResponse.StatusCode}.");
        }

        _logger.LogInformation("Earnings removed for omitted Learning {LearningKey}", removedResult.LearningKey);
    }

    private async Task HandleNewLearning(UpdateShortCourseLearningCommand command, ShortCourseOnProgramme onProg, UpdateShortCourseLearningPutResponse learningResponse, ShortCourseLookupResult courseDetails)
    {
        _logger.LogInformation("New {LearningOrEpisode} created for learner {LearnerKey} / course {CourseCode}. LearningKey: {LearningKey}",
            learningResponse.IsNewEpisode ? "Episode" : "Learning", command.LearnerKey, onProg.CourseCode, learningResponse.LearningKey);

        var resolvedOnProg = ResolveOnProgrammeFromLearningResponse(onProg, learningResponse);
        var earningsRequest = BuildCreateEarningsRequest(command, onProg, resolvedOnProg, learningResponse, courseDetails.Price, courseDetails.LearningType);
        await _earningsApiClient.Post(new PostCreateUnapprovedShortCourseLearningRequest(earningsRequest));

        var correlationId = Guid.NewGuid();
        await _messageSession.Publish(MapToLearnerDataEvent(command, onProg, resolvedOnProg, courseDetails.Price, correlationId));

        _logger.LogInformation("LearnerDataEvent published for Learning {LearningKey}", learningResponse.LearningKey);
    }

    private async Task HandleExistingLearning(UpdateShortCourseLearningCommand command, ShortCourseOnProgramme onProg, UpdateShortCourseLearningPutResponse learningResponse)
    {
        if (EarningsUpdateRequired(learningResponse))
        {
            var resolvedOnProg = ResolveOnProgrammeFromLearningResponse(onProg, learningResponse);
            var earningBody = _updateShortCourseOnProgrammeEarningPutRequestBuilder.Build(resolvedOnProg, learningResponse.LearnerKey, command.Request.Learner.LearnerRef);
            var earningRequest = new UpdateShortCourseOnProgrammeEarningPutRequest(learningResponse.LearningKey, learningResponse.UpdatedEpisodeKey, earningBody);
            await _earningsApiClient.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(earningRequest);
        }
        else
        {
            _logger.LogInformation("No earnings update required for {LearnerKey} / {CourseCode}", command.LearnerKey, onProg.CourseCode);
        }
    }

    private static LearningInnerOnProgramme ResolveOnProgrammeFromLearningResponse(ShortCourseOnProgramme onProg, UpdateShortCourseLearningPutResponse learningResponse)
    {
        var episode = learningResponse.Episodes.Single(e => e.EpisodeKey == learningResponse.UpdatedEpisodeKey);

        var milestones = onProg.Milestones.ToList();
        if (episode.CompletionDate.HasValue && !milestones.Contains(Milestone.LearningComplete))
            milestones.Add(Milestone.LearningComplete);

        return new LearningInnerOnProgramme
        {
            CourseCode = episode.CourseCode,
            Ukprn = episode.Ukprn,
            StartDate = episode.StartDate,
            ExpectedEndDate = episode.PlannedEndDate,
            CompletionDate = episode.CompletionDate,
            WithdrawalDate = episode.WithdrawalDate,
            WithdrawalReasonCode = onProg.WithdrawalReasonCode,
            Milestones = milestones
        };
    }

    private UpdateShortCourseLearningPutRequest BuildLearningRequest(UpdateShortCourseLearningCommand command, IReadOnlyList<ShortCourseLookupResult> courseDetails)
    {
        var body = new UpdateShortCourseLearningRequestBody
        {
            Ukprn = command.Ukprn,
            AcademicYear = command.AcademicYear,
            LearnerUpdateDetails = new ShortCourseLearnerUpdateDetails
            {
                LearnerRef = command.Request.Learner.LearnerRef
            },
            OnProgramme = command.Request.Delivery.OnProgramme.Select((onProg, i) =>
            {
                var milestones = onProg.Milestones.Select(m => Enum.Parse<Milestone>(m.ToString())).ToList();
                if (onProg.CompletionDate.HasValue && !onProg.Milestones.Contains(Milestone.LearningComplete))
                    milestones.Add(Milestone.LearningComplete);

                return new ShortCourseOnProgrammeUpdateDetails
                {
                    Ukprn = command.Ukprn,
                    CourseCode = onProg.CourseCode,
                    StartDate = onProg.StartDate,
                    ExpectedEndDate = onProg.ExpectedEndDate,
                    CompletionDate = onProg.CompletionDate,
                    WithdrawalDate = onProg.WithdrawalDate,
                    WithdrawalReasonCode = onProg.WithdrawalReasonCode,
                    Milestones = milestones,
                    Price = courseDetails[i].Price,
                    LearningType = courseDetails[i].LearningType
                };
            }).ToList()
        };

        return new UpdateShortCourseLearningPutRequest(command.LearnerKey, body);
    }

    private CreateUnapprovedShortCourseLearningRequest BuildCreateEarningsRequest(
        UpdateShortCourseLearningCommand command,
        ShortCourseOnProgramme onProg,
        LearningInnerOnProgramme resolvedOnProg,
        UpdateShortCourseLearningPutResponse learningResponse,
        decimal price,
        SharedLearningType learningType)
    {
        return new CreateUnapprovedShortCourseLearningRequest
        {
            LearningKey = learningResponse.LearningKey,
            EpisodeKey = learningResponse.UpdatedEpisodeKey,
            Learner = new Learner
            {
                DateOfBirth = command.Request.Learner.Dob,
                Uln = command.Request.Learner.Uln.ToString()
            },
            LearningSupport = onProg.LearningSupport,
            OnProgramme = new EarningsOnProgramme
            {
                CourseCode = resolvedOnProg.CourseCode,
                Ukprn = resolvedOnProg.Ukprn,
                StartDate = resolvedOnProg.StartDate,
                ExpectedEndDate = resolvedOnProg.ExpectedEndDate,
                CompletionDate = resolvedOnProg.CompletionDate,
                WithdrawalDate = resolvedOnProg.WithdrawalDate,
                Milestones = resolvedOnProg.Milestones,
                TotalPrice = price,
                LearningType = learningType
            }
        };
    }

    private static LearnerDataEvent MapToLearnerDataEvent(UpdateShortCourseLearningCommand command, ShortCourseOnProgramme onProg, LearningInnerOnProgramme resolvedOnProg, decimal price, Guid correlationId)
    {
        return new LearnerDataEvent
        {
            ULN = command.Request.Learner.Uln,
            UKPRN = command.Ukprn,
            FirstName = command.Request.Learner.FirstName,
            LastName = command.Request.Learner.LastName,
            Email = command.Request.Learner.Email,
            DoB = command.Request.Learner.Dob,
            StartDate = resolvedOnProg.StartDate,
            PlannedEndDate = resolvedOnProg.ExpectedEndDate,
            PercentageLearningToBeDelivered = 100,
            EpaoPrice = 0,
            TrainingPrice = (int)price,
            IsFlexiJob = false,
            PlannedOTJTrainingHours = 0,
            AgreementId = onProg.AgreementId,
            StandardCode = 0,
            ConsumerReference = command.Request.ConsumerReference,
            LarsCode = resolvedOnProg.CourseCode,
            CorrelationId = correlationId,
            ReceivedDate = DateTime.UtcNow,
            LearningType = LearningType.ApprenticeshipUnit
        };
    }

    private static bool EarningsUpdateRequired(UpdateShortCourseLearningPutResponse response)
    {
        var changes = response.GetChangesEnums();

        return changes.Contains(ShortCourseUpdateChanges.WithdrawalDate) ||
            changes.Contains(ShortCourseUpdateChanges.Milestone) ||
            changes.Contains(ShortCourseUpdateChanges.CompletionDate) ||
            changes.Contains(ShortCourseUpdateChanges.Reinstated) ||
            changes.Contains(ShortCourseUpdateChanges.StartDate) ||
            changes.Contains(ShortCourseUpdateChanges.ExpectedEndDate);
    }
}
