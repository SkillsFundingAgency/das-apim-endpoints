using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.LearnerData.Application.CreateShortCourse;
using SFA.DAS.LearnerData.Application.Requests.Earnings;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;

namespace SFA.DAS.LearnerData.Application.CreateShortCourseLearning;

#pragma warning disable CS8618
public class CreateDraftShortCourseCommandHandler(
    ILogger<CreateDraftShortCourseCommandHandler> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
    ICreateDraftShortCoursePostRequestBuilder createDraftShortCoursePostRequestBuilder,
    ICreateUnapprovedShortCourseLearningRequestBuilder createUnapprovedShortCourseLearningRequestBuilder,
    IUpdateShortCourseOnProgrammeEarningPutRequestBuilder updateShortCourseOnProgrammeEarningPutRequestBuilder,
    IMessageSession messageSession
) : IRequestHandler<CreateDraftShortCourseCommand, CreateDraftShortCourseResult>
{
    public async Task<CreateDraftShortCourseResult> Handle(CreateDraftShortCourseCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating draft short course for provider {ProviderUkprn}", command.Ukprn);

        var requestData = await createDraftShortCoursePostRequestBuilder.Build(command.ShortCourseRequest, command.Ukprn, command.AcademicYear);

        var learningResponse = await learningApiClient.PostWithResponseCode<CreateDraftShortCoursePostResponse>(new CreateDraftShortCourseApiPostRequest(requestData));

        //Short-circuit where learning ignored the request due to temporary rules around unhandled scenarios
        if (learningResponse.StatusCode == HttpStatusCode.NoContent)
        {
            return new CreateDraftShortCourseResult();
        }

        var correlationId = Guid.NewGuid();

        foreach (var (onProg, resolvedOnProg, result) in command.ShortCourseRequest.Delivery.OnProgramme.Zip(requestData.OnProgramme, learningResponse.Body.Results))
        {
            if (result.IsIgnored)
            {
                logger.LogInformation("Ignoring OnProgramme item for CourseCode {CourseCode}", onProg.CourseCode);
                continue;
            }

            if (result.IsReinstated)
            {
                await HandleReinstatedLearning(command, resolvedOnProg, result);
                continue;
            }

            await HandleNewLearning(command, requestData, onProg, resolvedOnProg, result, correlationId);
        }

        foreach (var removedResult in learningResponse.Body.Results.Where(r => r.IsRemoved))
        {
            await HandleRemovedLearning(removedResult, command.ShortCourseRequest.Learner.LearnerRef);
        }

        return new CreateDraftShortCourseResult { CorrelationId = correlationId };
    }

    private async Task HandleRemovedLearning(CreateShortCoursePostResponse removedResult, string learnerRef)
    {
        logger.LogInformation("Removing omitted Learning {LearningKey} / {CourseCode} from Earnings",
            removedResult.LearningKey, removedResult.CourseCode);

        var earningsRequest = new DeleteShortCourseEarningsRequest(removedResult.LearningKey, removedResult.EpisodeKey, removedResult.LearnerKey, learnerRef);
        var earningsResponse = await earningsApiClient.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(earningsRequest, true);

        if (!earningsResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to delete earnings for omitted Learning {LearningKey}. Status: {StatusCode}",
                removedResult.LearningKey, earningsResponse.StatusCode);
            throw new Exception($"Failed to delete earnings for omitted Learning {removedResult.LearningKey}. Status: {earningsResponse.StatusCode}.");
        }

        logger.LogInformation("Earnings removed for omitted Learning {LearningKey}", removedResult.LearningKey);
    }

    private async Task HandleReinstatedLearning(CreateDraftShortCourseCommand command, OnProgramme resolvedOnProg, CreateShortCoursePostResponse result)
    {
        var earningsOnProg = ResolveOnProgrammeFromLearningResponse(resolvedOnProg, result);
        var earningsPutBody = updateShortCourseOnProgrammeEarningPutRequestBuilder.Build(earningsOnProg, result.LearnerKey, command.ShortCourseRequest.Learner.LearnerRef);
        await earningsApiClient.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(
            new UpdateShortCourseOnProgrammeEarningPutRequest(result.LearningKey, result.EpisodeKey, earningsPutBody));
    }

    private async Task HandleNewLearning(
        CreateDraftShortCourseCommand command,
        CreateDraftShortCourseRequest requestData,
        ShortCourseOnProgramme onProg,
        OnProgramme resolvedOnProg,
        CreateShortCoursePostResponse result,
        Guid correlationId)
    {
        var earningsOnProg = ResolveOnProgrammeFromLearningResponse(resolvedOnProg, result);
        var earningsRequestData = createUnapprovedShortCourseLearningRequestBuilder.Build(command.ShortCourseRequest, onProg, result.LearningKey, result.EpisodeKey, command.Ukprn, earningsOnProg);
        await earningsApiClient.Post(new SFA.DAS.LearnerData.Requests.EarningsInner.PostCreateUnapprovedShortCourseLearningRequest(earningsRequestData));

        await messageSession.Publish(MapToEvent(command.Ukprn, requestData, onProg, resolvedOnProg, command.ShortCourseRequest.ConsumerReference, correlationId));
    }

    private static OnProgramme ResolveOnProgrammeFromLearningResponse(OnProgramme resolvedOnProg, CreateShortCoursePostResponse result)
    {
        var episode = result.Episodes.Single(e => e.EpisodeKey == result.EpisodeKey);

        return new OnProgramme
        {
            CourseCode = episode.CourseCode,
            Ukprn = episode.Ukprn,
            EmployerId = resolvedOnProg.EmployerId,
            StartDate = episode.StartDate,
            ExpectedEndDate = episode.PlannedEndDate,
            CompletionDate = episode.CompletionDate,
            WithdrawalDate = episode.WithdrawalDate,
            WithdrawalReasonCode = resolvedOnProg.WithdrawalReasonCode,
            Milestones = resolvedOnProg.Milestones,
            Price = resolvedOnProg.Price,
            LearningType = resolvedOnProg.LearningType
        };
    }

    private static LearnerDataEvent MapToEvent(
        long ukprn,
        CreateDraftShortCourseRequest request,
        ShortCourseOnProgramme onProg,
        OnProgramme resolvedOnProg,
        string consumerReference,
        Guid correlationId)
    {
        return new LearnerDataEvent
        {
            ULN = request.LearnerUpdateDetails.Uln,
            UKPRN = ukprn,
            FirstName = request.LearnerUpdateDetails.FirstName,
            LastName = request.LearnerUpdateDetails.LastName,
            Email = request.LearnerUpdateDetails.EmailAddress,
            DoB = request.LearnerUpdateDetails.DateOfBirth,
            StartDate = resolvedOnProg.StartDate,
            PlannedEndDate = resolvedOnProg.ExpectedEndDate,
            PercentageLearningToBeDelivered = 100,
            EpaoPrice = 0,
            TrainingPrice = (int)resolvedOnProg.Price,
            IsFlexiJob = false,
            PlannedOTJTrainingHours = 0,
            AgreementId = onProg.AgreementId,
            StandardCode = 0,
            ConsumerReference = consumerReference,
            LarsCode = resolvedOnProg.CourseCode,
            CorrelationId = correlationId,
            ReceivedDate = DateTime.UtcNow,
            LearningType = (LearningType)resolvedOnProg.LearningType
        };
    }
}
#pragma warning restore CS8618
