using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Commands.UpsertProviderAllowedCourse;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.ProviderAllowedCoursesControllerTests;

public class ProviderAllowedCoursesControllerPostTests
{
    [Test, MoqAutoData]
    public async Task WhenUpsertProviderAllowedCourseIsInvoked_ThenReturnsCreatedResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderAllowedCoursesController sut,
        UpsertProviderAllowedCourseModel request,
        int ukprn,
        string larsCode)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.IsAny<UpsertProviderAllowedCourseCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await sut.UpsertProviderAllowedCourse(ukprn, larsCode, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test, MoqAutoData]
    public async Task WhenUpsertProviderAllowedCourseIsInvoked_ThenVerifyMediatorIsCalled(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderAllowedCoursesController sut,
        UpsertProviderAllowedCourseModel request,
        int ukprn,
        string larsCode)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(
                It.Is<UpsertProviderAllowedCourseCommand>(c =>
                    c.Ukprn == ukprn &&
                    c.LarsCode == larsCode &&
                    c.UserId == request.UserId &&
                    c.UserDisplayName == request.UserDisplayName &&
                    c.LastDateStarts == request.LastDateStarts),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await sut.UpsertProviderAllowedCourse(ukprn, larsCode, request);

        // Assert
        mediatorMock.Verify(x => x.Send(
            It.Is<UpsertProviderAllowedCourseCommand>(c =>
                c.Ukprn == ukprn &&
                c.LarsCode == larsCode &&
                c.UserId == request.UserId &&
                c.UserDisplayName == request.UserDisplayName &&
                c.LastDateStarts == request.LastDateStarts),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
