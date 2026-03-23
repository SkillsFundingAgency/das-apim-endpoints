using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Application.CreateShortCourse;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;

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

        var requestData = createDraftShortCoursePostRequestBuilder.Build(command.ShortCourseRequest, command.Ukprn);

        var learningResponse = await learningApiClient.PostWithResponseCode<CreateShortCoursePostResponse>(new CreateDraftShortCourseApiPostRequest(requestData));

        var earningsRequestData = createUnapprovedShortCourseLearningRequestBuilder.Build(command.ShortCourseRequest, learningResponse.Body.LearningKey, learningResponse.Body.EpisodeKey, command.Ukprn);

        await earningsApiClient.Post(new PostCreateUnapprovedShortCourseLearningRequest(earningsRequestData));

        //removed for now until downstream fixed
        //await messageSession.Publish(MapToEvent(command.Ukprn, requestData));

        return new CreateDraftShortCourseResult();
    }

    private static LearnerDataEvent MapToEvent(long ukprn, CreateDraftShortCourseRequest request)
    {
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
            TrainingPrice = (int)request.OnProgramme.Price,
            AgreementId = request.OnProgramme.EmployerId.ToString(),
            StandardCode = Convert.ToInt32(request.OnProgramme.CourseCode),
            ReceivedDate = DateTime.UtcNow,
            LearningType = LearningType.ApprenticeshipUnit
        };
    }
}
#pragma warning restore CS8618
