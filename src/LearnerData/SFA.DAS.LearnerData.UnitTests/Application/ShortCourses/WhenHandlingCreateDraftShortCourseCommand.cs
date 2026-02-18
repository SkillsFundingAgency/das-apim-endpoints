using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;
using SFA.DAS.SharedOuterApi.Interfaces;

[TestFixture]
public class WhenHandlingCreateDraftShortCourseCommand
{
    private CreateDraftShortCourseCommandHandler _handler;
    private Mock<ILogger<CreateDraftShortCourseCommandHandler>> _logger;
    private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
    private Mock<ICreateDraftShortCoursePostRequestBuilder> _createDraftShortCoursePostRequestBuilder;
    private Mock<IMessageSession> _messageSession;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateDraftShortCourseCommandHandler>>();
        _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _createDraftShortCoursePostRequestBuilder = new Mock<ICreateDraftShortCoursePostRequestBuilder>();
        _messageSession = new Mock<IMessageSession>();

        _handler = new CreateDraftShortCourseCommandHandler(
            _logger.Object,
            _learningApiClient.Object,
            _createDraftShortCoursePostRequestBuilder.Object,
            _messageSession.Object);
    }

    [Test]
    public async Task Then_Learning_Is_Updated_With_ShortCourse()
    {
        // Arrange
        var ukprn = 12345;
        var shortCourseRequest = new ShortCourseRequest();
        var command = new CreateDraftShortCourseCommand
        {
            Ukprn = ukprn,
            ShortCourseRequest = shortCourseRequest
        };

        var builtRequest = new CreateDraftShortCourseRequest
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
            .Setup(x => x.Build(shortCourseRequest, ukprn))
            .Returns(builtRequest);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x =>
            x.PostWithResponseCode<Guid>(
                It.Is<CreateDraftShortCourseApiPostRequest>(r => r.Data == builtRequest), true),
            Times.Once);
    }

    [Test]
    public async Task Then_A_LearnerData_Event_Is_Emitted()
    {
        // Arrange
        var ukprn = 12345;
        var shortCourseRequest = new ShortCourseRequest();
        var command = new CreateDraftShortCourseCommand
        {
            Ukprn = ukprn,
            ShortCourseRequest = shortCourseRequest
        };

        var builtRequest = new CreateDraftShortCourseRequest
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
            .Setup(x => x.Build(shortCourseRequest, ukprn))
            .Returns(builtRequest);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _messageSession.Verify(x =>
            x.Publish(
                It.Is<LearnerDataEvent>(e =>
                    e.ULN == builtRequest.LearnerUpdateDetails.Uln &&
                    e.UKPRN == ukprn &&
                    e.FirstName == builtRequest.LearnerUpdateDetails.FirstName &&
                    e.LastName == builtRequest.LearnerUpdateDetails.LastName &&
                    e.Email == builtRequest.LearnerUpdateDetails.EmailAddress &&
                    e.StartDate == builtRequest.OnProgramme.StartDate &&
                    e.PlannedEndDate == builtRequest.OnProgramme.ExpectedEndDate &&
                    e.StandardCode == Convert.ToInt32(builtRequest.OnProgramme.CourseCode)
                ),
                It.IsAny<PublishOptions>()),
            Times.Once);
    }
}