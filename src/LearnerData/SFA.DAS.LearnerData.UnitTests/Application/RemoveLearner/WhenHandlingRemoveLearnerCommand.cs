using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.RemoveLearner;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
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
        var startDate = DateTime.UtcNow;

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<RemoveLearnerResponse>(
                It.Is<RemoveLearnerApiDeleteRequest>(r => r.LearningKey == command.LearningKey), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<RemoveLearnerResponse>(new RemoveLearnerResponse{ LastDayOfLearning = startDate }, HttpStatusCode.NoContent, ""));

        _earningsApiClient.Setup(x => x.PatchWithResponseCode<WithdrawLearnerRequest>(
                It.IsAny<WithdrawLearnerPatchRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.NoContent, ""));

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x => x.DeleteWithResponseCode<RemoveLearnerResponse>(
            It.Is<RemoveLearnerApiDeleteRequest>(r => r.LearningKey == command.LearningKey && r.Ukprn == command.Ukprn), It.IsAny<bool>()), Times.Once);

        _earningsApiClient.Verify(x => x.PatchWithResponseCode<WithdrawLearnerRequest>(
            It.Is<WithdrawLearnerPatchRequest>(r => r.LearningKey == command.LearningKey && r.Data.WithdrawalDate == startDate)), Times.Once);
    }

    [Test]
    public void Then_Throws_If_No_StartDate_Found()
    {
        // Arrange
        var command = _fixture.Create<RemoveLearnerCommand>();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<RemoveLearnerResponse>(
                It.Is<RemoveLearnerApiDeleteRequest>(r => r.LearningKey == command.LearningKey), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<RemoveLearnerResponse>(null, HttpStatusCode.NoContent, ""));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }

    [Test]
    public void Then_Throws_If_Delete_Fails()
    {
        // Arrange
        var command = _fixture.Create<RemoveLearnerCommand>();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<RemoveLearnerResponse>(
                It.IsAny<RemoveLearnerApiDeleteRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<RemoveLearnerResponse>(null, HttpStatusCode.InternalServerError, ""));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }

    [Test]
    public void Then_Throws_If_Earnings_Patch_Fails()
    {
        // Arrange
        var command = _fixture.Create<RemoveLearnerCommand>();
        var startDate = DateTime.UtcNow;

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<RemoveLearnerResponse>(
                It.Is<RemoveLearnerApiDeleteRequest>(r => r.LearningKey == command.LearningKey), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<RemoveLearnerResponse>(new RemoveLearnerResponse { LastDayOfLearning = startDate }, HttpStatusCode.NoContent, ""));

        _earningsApiClient.Setup(x => x.PatchWithResponseCode<WithdrawLearnerRequest>(
                It.IsAny<WithdrawLearnerPatchRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.InternalServerError, ""));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }
}
