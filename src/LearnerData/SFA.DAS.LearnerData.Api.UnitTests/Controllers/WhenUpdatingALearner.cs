using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Api.Controllers;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.LearnerData.Api.UnitTests.Controllers;

public class WhenUpdatingALearner
{
    [Test, MoqAutoData]
    public async Task And_when_successful_Then_Accepted_returned(
        Guid learningKey,
        UpdateLearnerRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<LearnersController>> mockLogger,
        [Greedy] LearnersController sut)
    {
        // Act
        var result = await sut.UpdateLearner(Guid.Empty, learningKey, request) as AcceptedResult;

        // Assert
        result!.StatusCode.Should().Be((int)HttpStatusCode.Accepted);

        mockMediator.Verify(x => x.Send(
            It.Is<UpdateLearnerCommand>(c =>
                c.LearningKey == learningKey &&
                c.UpdateLearnerRequest == request), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_when_exception_thrown_Then_InternalServerError_returned(
        Guid learningKey,
        UpdateLearnerRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<LearnersController>> mockLogger,
        [Greedy] LearnersController sut)
    {
        // Arrange
        mockMediator.Setup(x => x.Send(It.IsAny<UpdateLearnerCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Something went wrong"));

        // Act
        var result = await sut.UpdateLearner(Guid.Empty, learningKey, request) as StatusCodeResult;

        // Assert
        result!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

        mockLogger.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains($"Internal error occurred when updating learner {learningKey}")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}

