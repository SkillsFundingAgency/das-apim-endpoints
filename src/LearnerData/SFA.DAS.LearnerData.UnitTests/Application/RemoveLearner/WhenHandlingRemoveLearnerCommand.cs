using AutoFixture;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.LearnerData.Application.RemoveLearner;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;

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
        var removedLearningKeys = _fixture.CreateMany<Guid>(2).ToList();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<List<Guid>>(
                It.Is<RemoveLearnerApiDeleteRequest>(r => r.LearnerKey == command.LearnerKey), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<List<Guid>>(removedLearningKeys, HttpStatusCode.OK, ""));

        _earningsApiClient.Setup(x => x.DeleteWithResponseCode<NullResponse>(
                It.IsAny<DeleteLearningRequest>(), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.NoContent, ""));

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x => x.DeleteWithResponseCode<List<Guid>>(
            It.Is<RemoveLearnerApiDeleteRequest>(r => r.LearnerKey == command.LearnerKey && r.Ukprn == command.Ukprn), It.IsAny<bool>()), Times.Once);

        _earningsApiClient.Verify(x => x.DeleteWithResponseCode<NullResponse>(
            It.Is<DeleteLearningRequest>(r => removedLearningKeys.Contains(r.LearningKey)), false), Times.Exactly(removedLearningKeys.Count));
    }

    [Test]
    public void Then_Throws_If_Delete_Fails()
    {
        // Arrange
        var command = _fixture.Create<RemoveLearnerCommand>();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<List<Guid>>(
                It.IsAny<RemoveLearnerApiDeleteRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<List<Guid>>(null, HttpStatusCode.InternalServerError, ""));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }

    [Test]
    public void Then_Throws_If_Earnings_Delete_Fails()
    {
        // Arrange
        var command = _fixture.Create<RemoveLearnerCommand>();
        var removedLearningKeys = _fixture.CreateMany<Guid>(2).ToList();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<List<Guid>>(
                It.Is<RemoveLearnerApiDeleteRequest>(r => r.LearnerKey == command.LearnerKey), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<List<Guid>>(removedLearningKeys, HttpStatusCode.OK, ""));

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

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<List<Guid>>(
                It.IsAny<RemoveLearnerApiDeleteRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<List<Guid>>(null, HttpStatusCode.OK, ""));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }
}
