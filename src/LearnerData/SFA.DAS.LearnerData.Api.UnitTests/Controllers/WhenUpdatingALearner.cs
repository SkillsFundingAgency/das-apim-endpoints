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
using SFA.DAS.LearnerData.Services;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.LearnerData.Api.UnitTests.Controllers;

public class WhenUpdatingALearner
{
    [Test, MoqAutoData]
    public async Task And_when_successful_Then_Accepted_returned(
        Guid learnerKey,
        long ukprn,
        UpdateLearnerRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<LearnersController>> mockLogger,
        [Greedy] LearnersController sut)
    {
        // Act
        var result = await sut.UpdateLearner(ukprn, learnerKey, request) as AcceptedResult;

        // Assert
        result!.StatusCode.Should().Be((int)HttpStatusCode.Accepted);

        mockMediator.Verify(x => x.Send(
            It.Is<UpdateLearnerCommand>(c =>
                c.LearnerKey == learnerKey &&
                c.UpdateLearnerRequest == request), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_when_exception_thrown_Then_InternalServerError_returned(
        Guid learnerKey,
        long ukprn,
        UpdateLearnerRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<LearnersController>> mockLogger,
        [Greedy] LearnersController sut)
    {
        // Arrange
        mockMediator.Setup(x => x.Send(It.IsAny<UpdateLearnerCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Something went wrong"));

        // Act
        var result = await sut.UpdateLearner(ukprn, learnerKey, request) as StatusCodeResult;

        // Assert
        result!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

        mockLogger.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains($"Internal error occurred when updating learner {learnerKey}")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_when_english_and_maths_value_not_found_Then_UnprocessableEntity_returned(
        Guid learnerKey,
        long ukprn,
        UpdateLearnerRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LearnersController sut)
    {
        // Arrange
        mockMediator.Setup(x => x.Send(It.IsAny<UpdateLearnerCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EnglishAndMathsValueNotFoundException("60005105", new DateTime(2025, 8, 1)));

        // Act
        var result = await sut.UpdateLearner(ukprn, learnerKey, request) as StatusCodeResult;

        // Assert
        result!.StatusCode.Should().Be((int)HttpStatusCode.UnprocessableEntity);
    }

    [Test, MoqAutoData]
    public async Task And_when_english_and_maths_value_unavailable_Then_InternalServerError_returned(
        Guid learnerKey,
        long ukprn,
        UpdateLearnerRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LearnersController sut)
    {
        // Arrange
        mockMediator.Setup(x => x.Send(It.IsAny<UpdateLearnerCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EnglishAndMathsValueUnavailableException("SLD did not return a value.", HttpStatusCode.ServiceUnavailable));

        // Act
        var result = await sut.UpdateLearner(ukprn, learnerKey, request) as StatusCodeResult;

        // Assert
        result!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
