using System.Net;
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

namespace SFA.DAS.LearnerData.Api.UnitTests.Controllers;

public class WhenCreatingDraftShortCourse
{
    [Test]
    [MoqInlineAutoData(HttpStatusCode.OK)]
    [MoqInlineAutoData(HttpStatusCode.Created)]
    [MoqInlineAutoData(HttpStatusCode.Accepted)]
    public async Task Then_If_Success_Returned_From_Handler_Then_Accepted_Returned(
        HttpStatusCode statusCode,
        long ukprn,
        ShortCourseRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<ShortCoursesController>> mockLogger,
        [Greedy] ShortCoursesController sut)
    {
        // Arrange
        var response = new CreateDraftShortCourseResult { StatusCode = statusCode };
        mockMediator
            .Setup(x => x.Send(It.Is<CreateDraftShortCourseCommand>(c =>
                c.Ukprn == ukprn &&
                c.ShortCourseRequest == request), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.CreateShortCourse(request, ukprn) as AcceptedResult;

        // Assert
        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Conflict_Returned_From_Handler_Then_Conflict_Returned(
        long ukprn,
        ShortCourseRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<ShortCoursesController>> mockLogger,
        [Greedy] ShortCoursesController sut)
    {
        // Arrange
        var response = new CreateDraftShortCourseResult { StatusCode = HttpStatusCode.Conflict };
        mockMediator
            .Setup(x => x.Send(It.IsAny<CreateDraftShortCourseCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.CreateShortCourse(request, ukprn) as ConflictResult;

        // Assert
        result.Should().NotBeNull();
    }

    [Test]
    [MoqInlineAutoData(HttpStatusCode.BadRequest)]
    [MoqInlineAutoData(HttpStatusCode.NotFound)]
    [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
    public async Task Then_If_Error_Returned_From_Handler_Then_500_Returned(
        HttpStatusCode statusCode,
        long ukprn,
        ShortCourseRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<ShortCoursesController>> mockLogger,
        [Greedy] ShortCoursesController sut)
    {
        // Arrange
        var response = new CreateDraftShortCourseResult { StatusCode = statusCode };
        mockMediator
            .Setup(x => x.Send(It.IsAny<CreateDraftShortCourseCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.CreateShortCourse(request, ukprn) as StatusCodeResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Thrown_Then_500_Returned(
        long ukprn,
        ShortCourseRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<ShortCoursesController>> mockLogger,
        [Greedy] ShortCoursesController sut)
    {
        // Arrange
        mockMediator
            .Setup(x => x.Send(It.IsAny<CreateDraftShortCourseCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await sut.CreateShortCourse(request, ukprn) as StatusCodeResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
