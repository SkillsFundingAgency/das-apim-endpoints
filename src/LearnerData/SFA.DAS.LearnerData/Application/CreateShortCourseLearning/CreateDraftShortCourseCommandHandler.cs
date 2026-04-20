using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Application.CreateShortCourse;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
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
    IMessageSession messageSession
) : IRequestHandler<CreateDraftShortCourseCommand, CreateDraftShortCourseResult>
{
    public async Task<CreateDraftShortCourseResult> Handle(CreateDraftShortCourseCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating draft short course for provider {ProviderUkprn}", command.Ukprn);

        var requestData = await createDraftShortCoursePostRequestBuilder.Build(command.ShortCourseRequest, command.Ukprn);

        var learningResponse = await learningApiClient.PostWithResponseCode<CreateShortCoursePostResponse>(new CreateDraftShortCourseApiPostRequest(requestData));

        //Short-circuit where learning ignored the request due to temporary rules around unhandled scenarios
        if (learningResponse.StatusCode == HttpStatusCode.NoContent)
        {
            return new CreateDraftShortCourseResult();
        }

        var earningsRequestData = createUnapprovedShortCourseLearningRequestBuilder.Build(command.ShortCourseRequest, learningResponse.Body.LearningKey, learningResponse.Body.EpisodeKey, command.Ukprn, requestData);

        await earningsApiClient.Post(new SFA.DAS.LearnerData.Requests.EarningsInner.PostCreateUnapprovedShortCourseLearningRequest(earningsRequestData));

        var correlationId = Guid.NewGuid();
        await messageSession.Publish(MapToEvent(command.Ukprn, requestData, command.ShortCourseRequest, correlationId));

        return new CreateDraftShortCourseResult { CorrelationId = correlationId };
    }

    private static LearnerDataEvent MapToEvent(long ukprn, CreateDraftShortCourseRequest request, ShortCourseRequest shortCourseRequest, Guid correlationId)
    {
        var firstOnProg = shortCourseRequest.Delivery.OnProgramme.MinBy(x => x.StartDate);

        return new LearnerDataEvent
        {
            ULN = request.LearnerUpdateDetails.Uln,
            UKPRN = ukprn,
            FirstName = request.LearnerUpdateDetails.FirstName,
            LastName = request.LearnerUpdateDetails.LastName,
            Email = request.LearnerUpdateDetails.EmailAddress,
            DoB = request.LearnerUpdateDetails.DateOfBirth,
            StartDate = request.OnProgramme.StartDate,
            PlannedEndDate = request.OnProgramme.ExpectedEndDate,
            PercentageLearningToBeDelivered = 100,
            EpaoPrice = 0,
            TrainingPrice = (int)request.OnProgramme.Price,
            IsFlexiJob = false,
            PlannedOTJTrainingHours = 0,
            AgreementId = firstOnProg?.AgreementId,
            StandardCode = 0,
            ConsumerReference = shortCourseRequest.ConsumerReference,
            LarsCode = request.OnProgramme.CourseCode,
            CorrelationId = correlationId,
            ReceivedDate = DateTime.UtcNow,
            LearningType = (LearningType)request.OnProgramme.LearningType
        };
    }
}
#pragma warning restore CS8618
