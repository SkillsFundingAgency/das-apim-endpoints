using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

[TestFixture]
public class WhenHandlingCreateDraftShortCourseCommand
{
    private CreateDraftShortCourseCommandHandler _handler;
    private Mock<ILogger<CreateDraftShortCourseCommandHandler>> _logger;
    private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
    private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;
    private Mock<ICreateDraftShortCoursePostRequestBuilder> _createDraftShortCoursePostRequestBuilder;
    private Mock<ICreateUnapprovedShortCourseLearningRequestBuilder> _createUnapprovedShortCourseLearningRequestBuilder;
    private Mock<IMessageSession> _messageSession;

    private CreateDraftShortCourseCommand _command;
    private CreateDraftShortCourseRequest _builtRequest;
    private long _ukprn;
    private Guid _learningKey;
    private ShortCourseRequest _shortCourseRequest;
    private CreateUnapprovedShortCourseLearningRequest _builtEarningsRequest;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateDraftShortCourseCommandHandler>>();
        _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _earningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        _createDraftShortCoursePostRequestBuilder = new Mock<ICreateDraftShortCoursePostRequestBuilder>();
        _createUnapprovedShortCourseLearningRequestBuilder = new Mock<ICreateUnapprovedShortCourseLearningRequestBuilder>();
        _messageSession = new Mock<IMessageSession>();

        _handler = new CreateDraftShortCourseCommandHandler(
            _logger.Object,
            _learningApiClient.Object,
            _earningsApiClient.Object,
            _createDraftShortCoursePostRequestBuilder.Object,
            _createUnapprovedShortCourseLearningRequestBuilder.Object,
            _messageSession.Object);

        // Arrange
        _ukprn = 12345;
        _learningKey = Guid.NewGuid();
        
        _builtEarningsRequest = new CreateUnapprovedShortCourseLearningRequest();
        
        _shortCourseRequest = new ShortCourseRequest();

        _command = new CreateDraftShortCourseCommand
        {
            Ukprn = _ukprn,
            ShortCourseRequest = _shortCourseRequest
        };

        _builtRequest = new CreateDraftShortCourseRequest
        {
            LearnerUpdateDetails = new()
            {
                Uln = 99999999,
                FirstName = "John",
                LastName = "Smith",
                EmailAddress = "john@test.com",
                DateOfBirth = new DateTime(2000, 1, 1)
            },
            OnProgramme = new()
            {
                StartDate = new DateTime(2025, 8, 1),
                ExpectedEndDate = new DateTime(2026, 7, 31),
                Price = 1500,
                EmployerId = 123456,
                CourseCode = "91"
            }
        };

        _createDraftShortCoursePostRequestBuilder
            .Setup(x => x.Build(_shortCourseRequest, _ukprn))
            .Returns(_builtRequest);

        _learningApiClient
            .Setup(x => x.PostWithResponseCode<Guid>(
                It.IsAny<CreateDraftShortCourseApiPostRequest>(), true))
            .ReturnsAsync(new ApiResponse<Guid>(_learningKey, System.Net.HttpStatusCode.OK, string.Empty));

        _createUnapprovedShortCourseLearningRequestBuilder
            .Setup(x => x.Build(_shortCourseRequest, _learningKey, _ukprn))
            .Returns(_builtEarningsRequest);
    }

    [Test]
    public async Task Then_Learning_Is_Updated_With_ShortCourse()
    {
        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x =>
            x.PostWithResponseCode<Guid>(
                It.Is<CreateDraftShortCourseApiPostRequest>(r => r.Data == _builtRequest), true),
            Times.Once);
    }

    [Test]
    public async Task Then_A_LearnerData_Event_Is_Emitted()
    {
        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _messageSession.Verify(x =>
            x.Publish(
                It.Is<LearnerDataEvent>(e =>
                    e.ULN == _builtRequest.LearnerUpdateDetails.Uln &&
                    e.UKPRN == _ukprn &&
                    e.FirstName == _builtRequest.LearnerUpdateDetails.FirstName &&
                    e.LastName == _builtRequest.LearnerUpdateDetails.LastName &&
                    e.Email == _builtRequest.LearnerUpdateDetails.EmailAddress &&
                    e.StartDate == _builtRequest.OnProgramme.StartDate &&
                    e.PlannedEndDate == _builtRequest.OnProgramme.ExpectedEndDate &&
                    e.StandardCode == Convert.ToInt32(_builtRequest.OnProgramme.CourseCode)
                ),
                It.IsAny<PublishOptions>()),
            Times.Once);
    }

    [Test]
    public async Task Then_Earnings_Is_Updated_With_Unapproved_ShortCourse_Learning()
    {
        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _createUnapprovedShortCourseLearningRequestBuilder.Verify(x =>
                x.Build(_shortCourseRequest, _learningKey, _ukprn),
            Times.Once);

        _earningsApiClient.Verify(x =>
                x.Post(It.Is<PostCreateUnapprovedShortCourseLearningRequest>(
                    r => r.Data == _builtEarningsRequest)),
            Times.Once);
    }
}