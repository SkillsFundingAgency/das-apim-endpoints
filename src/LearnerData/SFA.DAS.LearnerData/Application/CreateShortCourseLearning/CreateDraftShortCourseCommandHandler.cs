using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Application.CreateShortCourse;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Application.CreateShortCourseLearning;

#pragma warning disable CS8618
public class CreateDraftShortCourseCommandHandler(
    ILogger<CreateDraftShortCourseCommandHandler> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
    ICreateDraftShortCoursePostRequestBuilder createDraftShortCoursePostRequestBuilder,
    ICreateUnapprovedShortCourseLearningRequestBuilder createUnapprovedShortCourseLearningRequestBuilder,
    IUpdateShortCourseOnProgrammeEarningPutRequestBuilder updateShortCourseOnProgrammeEarningPutRequestBuilder,
    ICalculateGrowthAndSkillsPaymentsEventBuilder calculateGrowthAndSkillsPaymentsEventBuilder,
    IMessageSession messageSession,
    PaymentsConfiguration paymentsConfiguration
) : IRequestHandler<CreateDraftShortCourseCommand, CreateDraftShortCourseResult>
{
    public async Task<CreateDraftShortCourseResult> Handle(CreateDraftShortCourseCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating draft short course for provider {ProviderUkprn}", command.Ukprn);

        var requestData = await createDraftShortCoursePostRequestBuilder.Build(command.ShortCourseRequest, command.Ukprn);

        var learningResponse = await learningApiClient.PostWithResponseCode<CreateDraftShortCoursePostResponse>(new CreateDraftShortCourseApiPostRequest(requestData));

        //Short-circuit where learning ignored the request due to temporary rules around unhandled scenarios
        if (learningResponse.StatusCode == HttpStatusCode.NoContent)
        {
            return new CreateDraftShortCourseResult();
        }

        var correlationId = Guid.NewGuid();

        for (var i = 0; i < learningResponse.Body.Results.Count; i++)
        {
            var onProg = command.ShortCourseRequest.Delivery.OnProgramme[i];
            var resolvedOnProg = requestData.OnProgramme[i];
            var result = learningResponse.Body.Results[i];

            if (result.IsIgnored)
            {
                logger.LogInformation("Ignoring OnProgramme item for CourseCode {CourseCode}", onProg.CourseCode);
                continue;
            }

            if (result.IsReinstated)
            {
                await HandleReinstatedLearning(command.Ukprn, resolvedOnProg, result);
                continue;
            }

            await HandleNewLearning(command, requestData, onProg, resolvedOnProg, result, correlationId);
        }

        return new CreateDraftShortCourseResult { CorrelationId = correlationId };
    }

    private async Task HandleReinstatedLearning(long ukprn, SFA.DAS.LearnerData.Requests.LearningInner.OnProgramme resolvedOnProg, CreateShortCoursePostResponse result)
    {
        var earningsPutBody = updateShortCourseOnProgrammeEarningPutRequestBuilder.Build(resolvedOnProg);
        var earningsResponse = await earningsApiClient.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(
            new UpdateShortCourseOnProgrammeEarningPutRequest(result.LearningKey, result.EpisodeKey, earningsPutBody));

        await PublishPaymentsEventForReinstatement(ukprn, result, earningsResponse.Body);
    }

    private async Task HandleNewLearning(
        CreateDraftShortCourseCommand command,
        CreateDraftShortCourseRequest requestData,
        ShortCourseOnProgramme onProg,
        SFA.DAS.LearnerData.Requests.LearningInner.OnProgramme resolvedOnProg,
        CreateShortCoursePostResponse result,
        Guid correlationId)
    {
        var earningsRequestData = createUnapprovedShortCourseLearningRequestBuilder.Build(command.ShortCourseRequest, onProg, result.LearningKey, result.EpisodeKey, command.Ukprn, resolvedOnProg);
        await earningsApiClient.Post(new SFA.DAS.LearnerData.Requests.EarningsInner.PostCreateUnapprovedShortCourseLearningRequest(earningsRequestData));

        await messageSession.Publish(MapToEvent(command.Ukprn, requestData, onProg, resolvedOnProg, command.ShortCourseRequest.ConsumerReference, correlationId));
    }

    private async Task PublishPaymentsEventForReinstatement(long ukprn, CreateShortCoursePostResponse learningResponse, UpdateShortCourseEarningPutResponse earningsResponse)
    {
        logger.LogInformation("Sending CalculateGrowthAndSkillsPayments command for reinstated LearningKey: {LearningKey}", learningResponse.LearningKey);

        var command = await calculateGrowthAndSkillsPaymentsEventBuilder.Build(ukprn, learningResponse, earningsResponse);

        var options = new SendOptions();
        options.DoNotEnforceBestPractices();
        options.SetDestination(paymentsConfiguration.PaymentsEndpoint);
        await messageSession.Send(command, options);

        await messageSession.Publish(new GrowthAndSkillsPaymentsRecalculatedEvent { Command = command });

        logger.LogInformation("CalculateGrowthAndSkillsPayments command sent for reinstated LearningKey: {LearningKey}", learningResponse.LearningKey);
    }

    private static LearnerDataEvent MapToEvent(
        long ukprn,
        CreateDraftShortCourseRequest request,
        ShortCourseOnProgramme onProg,
        SFA.DAS.LearnerData.Requests.LearningInner.OnProgramme resolvedOnProg,
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
