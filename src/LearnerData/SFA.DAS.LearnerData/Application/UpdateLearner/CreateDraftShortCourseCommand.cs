using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.UpdateLearner;

public class CreateDraftShortCourseCommand : IRequest<CreateDraftShortCourseResult>
{
    public long Ukprn { get; set; }
    public ShortCourseRequest ShortCourseRequest { get; set; }
}

public class CreateDraftShortCourseResult
{
    public HttpStatusCode StatusCode { get; set; }
}

public class CreateDraftShortCourseCommandHandler(
    ILogger<CreateDraftShortCourseCommandHandler> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    ICreateDraftShortCoursePostRequestBuilder createDraftShortCoursePostRequestBuilder,
    IMessageSession messageSession
) : IRequestHandler<CreateDraftShortCourseCommand, CreateDraftShortCourseResult>
{
    public async Task<CreateDraftShortCourseResult> Handle(CreateDraftShortCourseCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating draft short course for provider {ProviderUkprn}", command.Ukprn);

        var requestData = createDraftShortCoursePostRequestBuilder.Build(command.ShortCourseRequest, command.Ukprn);

        var result = await learningApiClient.PostWithResponseCode<Guid>(new CreateDraftShortCourseApiPostRequest(requestData));

        //removed for now until downstream fixed
        //await messageSession.Publish(MapToEvent(command.Ukprn, requestData));

        return new CreateDraftShortCourseResult
        {
            StatusCode = result.StatusCode
        };
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