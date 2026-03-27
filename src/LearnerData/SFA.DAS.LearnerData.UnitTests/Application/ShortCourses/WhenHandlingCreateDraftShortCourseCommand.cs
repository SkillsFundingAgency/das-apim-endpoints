using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Application.CreateShortCourseLearning;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LearnerData.UnitTests.Application.ShortCourses;

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
    private Guid _episodeKey;
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
        _episodeKey = Guid.NewGuid();

        _builtEarningsRequest = new CreateUnapprovedShortCourseLearningRequest();

        _shortCourseRequest = new ShortCourseRequest
        {
            Delivery = new ShortCourseDelivery
            {
                OnProgramme = [new ShortCourseOnProgramme { StartDate = new DateTime(2025, 8, 1), AgreementId = "AGR-001" }]
            },
            ConsumerReference = "consumer-ref-123"
        };

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
            .ReturnsAsync(_builtRequest);

        var apiResponse = new ApiResponse<CreateShortCoursePostResponse>(
            new CreateShortCoursePostResponse { LearningKey = _learningKey, EpisodeKey = _episodeKey },
            HttpStatusCode.Created, "");

        _learningApiClient
            .Setup(x => x.PostWithResponseCode<CreateShortCoursePostResponse>(It.IsAny<CreateDraftShortCourseApiPostRequest>(), true))
            .ReturnsAsync(apiResponse);

        _createUnapprovedShortCourseLearningRequestBuilder
            .Setup(x => x.Build(_shortCourseRequest, _learningKey, _episodeKey, _ukprn, _builtRequest))
            .Returns(_builtEarningsRequest);
    }

    [Test]
    public async Task Then_Learning_Is_Updated_With_ShortCourse()
    {
        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x =>
                x.PostWithResponseCode<CreateShortCoursePostResponse>(
                    It.Is<CreateDraftShortCourseApiPostRequest>(r => r.Data == _builtRequest), true),
            Times.Once);
    }

    [Test]
    public async Task Then_A_LearnerDataEvent_Is_Published()
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
                    e.DoB == _builtRequest.LearnerUpdateDetails.DateOfBirth &&
                    e.StartDate == _builtRequest.OnProgramme.StartDate &&
                    e.PlannedEndDate == _builtRequest.OnProgramme.ExpectedEndDate &&
                    e.PercentageLearningToBeDelivered == 100 &&
                    e.EpaoPrice == 0 &&
                    e.TrainingPrice == (int)_builtRequest.OnProgramme.Price &&
                    e.IsFlexiJob == false &&
                    e.PlannedOTJTrainingHours == 0 &&
                    e.AgreementId == _shortCourseRequest.Delivery.OnProgramme.MinBy(x => x.StartDate)!.AgreementId &&
                    e.ConsumerReference == _shortCourseRequest.ConsumerReference &&
                    e.StandardCode == 0 &&
                    e.LarsCode == _builtRequest.OnProgramme.CourseCode &&
                    e.CorrelationId != Guid.Empty &&
                    e.LearningType == (LearningType)_builtRequest.OnProgramme.LearningType
                ),
                It.IsAny<PublishOptions>()),
            Times.Once);
    }

    [Test]
    public async Task Then_The_Result_CorrelationId_Matches_The_Published_Event()
    {
        // Arrange
        LearnerDataEvent publishedEvent = null;
        _messageSession
            .Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<PublishOptions>()))
            .Callback<object, PublishOptions>((e, _) => publishedEvent = e as LearnerDataEvent)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(_command, CancellationToken.None);

        // Assert
        result.CorrelationId.Should().Be(publishedEvent!.CorrelationId);
    }

    [Test]
    public async Task Then_Earnings_Is_Updated_With_Unapproved_ShortCourse_Learning()
    {
        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x =>
            x.Post(It.IsAny<PostCreateUnapprovedShortCourseLearningRequest>()));
    }

    [Test]
    public async Task Then_When_Learning_Returns_NoContent_Earnings_Is_Not_Called()
    {
        // Arrange
        var noContentResponse = new ApiResponse<CreateShortCoursePostResponse>(null, HttpStatusCode.NoContent, "");
        _learningApiClient
            .Setup(x => x.PostWithResponseCode<CreateShortCoursePostResponse>(It.IsAny<CreateDraftShortCourseApiPostRequest>(), true))
            .ReturnsAsync(noContentResponse);

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x => x.Post(It.IsAny<PostCreateUnapprovedShortCourseLearningRequest>()), Times.Never);
    }

    [Test]
    public async Task Then_When_Learning_Returns_NoContent_A_Result_Is_Returned()
    {
        // Arrange
        var noContentResponse = new ApiResponse<CreateShortCoursePostResponse>(null, HttpStatusCode.NoContent, "");
        _learningApiClient
            .Setup(x => x.PostWithResponseCode<CreateShortCoursePostResponse>(It.IsAny<CreateDraftShortCourseApiPostRequest>(), true))
            .ReturnsAsync(noContentResponse);

        // Act
        var result = await _handler.Handle(_command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
    }
}