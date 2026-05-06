using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Api.Controllers;
using SFA.DAS.LearnerData.Application.DeleteShortCourse;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerData.Api.UnitTests.Controllers;

public class WhenDeletingAShortCourse
{
    [Test, MoqAutoData]
    public async Task And_when_successful_Then_Accepted_returned(
        long ukprn,
        Guid learningKey,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<ShortCoursesController>> mockLogger,
        [Greedy] ShortCoursesController sut)
    {
        var result = await sut.DeleteShortCourse(ukprn, learningKey) as AcceptedResult;

        result!.StatusCode.Should().Be((int)HttpStatusCode.Accepted);

        mockMediator.Verify(x => x.Send(
            It.Is<DeleteShortCourseCommand>(c =>
                c.Ukprn == ukprn &&
                c.LearningKey == learningKey), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_when_exception_thrown_Then_InternalServerError_returned(
        long ukprn,
        Guid learningKey,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<ShortCoursesController>> mockLogger,
        [Greedy] ShortCoursesController sut)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<DeleteShortCourseCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Something went wrong"));

        var result = await sut.DeleteShortCourse(ukprn, learningKey) as StatusCodeResult;

        result!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

        mockLogger.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Internal error occurred when deleting short course")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}
