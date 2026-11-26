using AutoFixture;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Application.RemoveLearner;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using System.Net;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.RemoveLearner;

public class WhenHandlingRemoveLearnerCommand
{
    private Fixture _fixture;

#pragma warning disable CS8618
    private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
    private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;
    private Mock<ILogger<RemoveLearnerCommandHandler>> _logger;
    private RemoveLearnerCommandHandler _sut;
#pragma warning restore CS8618

    public WhenHandlingRemoveLearnerCommand()
    {
        _fixture = new Fixture();
    }

    [SetUp]
    public void Setup()
    {
        _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _earningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        _logger = new Mock<ILogger<RemoveLearnerCommandHandler>>();
        _sut = new RemoveLearnerCommandHandler(
            _logger.Object,
            _learningApiClient.Object,
            _earningsApiClient.Object);
    }

    [Test]
    public async Task Then_Learner_Is_Removed_Successfully()
    {
        // Arrange
        var command = _fixture.Create<RemoveLearnerCommand>();
        var removedLearnings = _fixture.CreateMany<RemovedLearningResponse>(2).ToList();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<List<RemovedLearningResponse>>(
                It.Is<RemoveLearnerApiDeleteRequest>(r => r.LearnerKey == command.LearnerKey), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<List<RemovedLearningResponse>>(removedLearnings, HttpStatusCode.OK, ""));

        _earningsApiClient.Setup(x => x.DeleteWithResponseCode<NullResponse>(
                It.IsAny<DeleteLearningRequest>(), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.NoContent, ""));

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x => x.DeleteWithResponseCode<List<RemovedLearningResponse>>(
            It.Is<RemoveLearnerApiDeleteRequest>(r => r.LearnerKey == command.LearnerKey && r.Ukprn == command.Ukprn && r.AcademicYear == command.AcademicYear), It.IsAny<bool>()), Times.Once);

        _earningsApiClient.Verify(x => x.DeleteWithResponseCode<NullResponse>(
            It.Is<DeleteLearningRequest>(r => removedLearnings.Any(l => l.LearningKey == r.LearningKey)), false), Times.Exactly(removedLearnings.Count));
    }

    [Test]
    public void Then_Throws_If_Delete_Fails()
    {
        // Arrange
        var command = _fixture.Create<RemoveLearnerCommand>();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<List<RemovedLearningResponse>>(
                It.IsAny<RemoveLearnerApiDeleteRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<List<RemovedLearningResponse>>(null, HttpStatusCode.InternalServerError, ""));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }

    [Test]
    public void Then_Throws_If_Earnings_Delete_Fails()
    {
        // Arrange
        var command = _fixture.Create<RemoveLearnerCommand>();
        var removedLearnings = _fixture.CreateMany<RemovedLearningResponse>(2).ToList();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<List<RemovedLearningResponse>>(
                It.Is<RemoveLearnerApiDeleteRequest>(r => r.LearnerKey == command.LearnerKey), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<List<RemovedLearningResponse>>(removedLearnings, HttpStatusCode.OK, ""));

        _earningsApiClient.Setup(x => x.DeleteWithResponseCode<NullResponse>(
                It.IsAny<DeleteLearningRequest>(), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.InternalServerError, ""));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }

    [Test]
    public void Then_Throws_If_Learning_Response_Body_Is_Null()
    {
        // Arrange
        var command = _fixture.Create<RemoveLearnerCommand>();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<List<RemovedLearningResponse>>(
                It.IsAny<RemoveLearnerApiDeleteRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<List<RemovedLearningResponse>>(null, HttpStatusCode.OK, ""));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task Then_Earnings_Are_Released_To_Payments_For_Each_Removed_Learning()
    {
        // Arrange
        var command = _fixture.Create<RemoveLearnerCommand>();
        var removedLearnings = _fixture.CreateMany<RemovedLearningResponse>(2).ToList();

        SetupSuccessfulRemoval(command, removedLearnings);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        foreach (var removedLearning in removedLearnings)
        {
            _earningsApiClient.Verify(x => x.Post(
                    It.Is<ReleaseEarningsApiPostRequest>(r =>
                        r.PostUrl == $"learning/{removedLearning.LearningKey}/release-earnings" &&
                        r.Data.LearnerKey == command.LearnerKey &&
                        r.Data.LearnerRef == removedLearning.LearnerRef &&
                        r.Data.ReleaseType == ReleaseType.All &&
                        r.Data.EnglishAndMathsCourseKeys.Count == 0)),
                Times.Once);
        }
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task Then_Earnings_Are_Not_Released_When_The_Removed_Learning_Has_No_LearnerRef(string? learnerRef)
    {
        // Arrange
        var command = _fixture.Create<RemoveLearnerCommand>();
        var removedLearning = _fixture.Build<RemovedLearningResponse>().With(x => x.LearnerRef, learnerRef).Create();

        SetupSuccessfulRemoval(command, [removedLearning]);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x => x.DeleteWithResponseCode<NullResponse>(
            It.Is<DeleteLearningRequest>(r => r.LearningKey == removedLearning.LearningKey), false), Times.Once);
        _earningsApiClient.Verify(x => x.Post(It.IsAny<ReleaseEarningsApiPostRequest>()), Times.Never);
    }

    [Test]
    public void Then_Earnings_Are_Not_Released_If_Earnings_Delete_Fails()
    {
        // Arrange
        var command = _fixture.Create<RemoveLearnerCommand>();
        var removedLearnings = _fixture.CreateMany<RemovedLearningResponse>(1).ToList();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<List<RemovedLearningResponse>>(
                It.IsAny<RemoveLearnerApiDeleteRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<List<RemovedLearningResponse>>(removedLearnings, HttpStatusCode.OK, ""));

        _earningsApiClient.Setup(x => x.DeleteWithResponseCode<NullResponse>(
                It.IsAny<DeleteLearningRequest>(), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.InternalServerError, ""));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
        _earningsApiClient.Verify(x => x.Post(It.IsAny<ReleaseEarningsApiPostRequest>()), Times.Never);
    }

    private void SetupSuccessfulRemoval(RemoveLearnerCommand command, List<RemovedLearningResponse> removedLearnings)
    {
        _learningApiClient.Setup(x => x.DeleteWithResponseCode<List<RemovedLearningResponse>>(
                It.Is<RemoveLearnerApiDeleteRequest>(r => r.LearnerKey == command.LearnerKey), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<List<RemovedLearningResponse>>(removedLearnings, HttpStatusCode.OK, ""));

        _earningsApiClient.Setup(x => x.DeleteWithResponseCode<NullResponse>(
                It.IsAny<DeleteLearningRequest>(), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.NoContent, ""));

        _earningsApiClient.Setup(x => x.Post(It.IsAny<ReleaseEarningsApiPostRequest>()))
            .Returns(Task.CompletedTask);
    }
}
