using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Api.Controllers;
using SFA.DAS.LearnerData.Application.RemoveLearner;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerData.Api.UnitTests.Controllers;

public class WhenRemovingALearner
{
    [Test, MoqAutoData]
    public async Task And_when_successful_Then_NoContent_returned(
        Guid learningKey,
        long ukprn,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<LearnersController>> mockLogger,
        [Greedy] LearnersController sut)
    {
        // Act
        var result = await sut.RemoveLearner(ukprn, learningKey) as NoContentResult;

        // Assert
        result!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

        mockMediator.Verify(x => x.Send(
            It.Is<RemoveLearnerCommand>(c =>
                c.LearningKey == learningKey &&
                c.Ukprn == ukprn), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_when_exception_thrown_Then_InternalServerError_returned(
        Guid learningKey,
        long ukprn,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<LearnersController>> mockLogger,
        [Greedy] LearnersController sut)
    {
        // Arrange
        mockMediator.Setup(x => x.Send(It.IsAny<RemoveLearnerCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Something went wrong"));

        // Act
        var result = await sut.RemoveLearner(ukprn, learningKey) as StatusCodeResult;

        // Assert
        result!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

        mockLogger.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains($"Internal error occurred when removing learner {learningKey}")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}