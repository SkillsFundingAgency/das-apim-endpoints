using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Commands.PatchProviderAllowedCourse;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.ProviderAllowedCoursesControllerTests;

public class ProviderAllowedCoursesControllerPatchTests
{
    [Test, MoqAutoData]
    public async Task WhenPatchProviderAllowedCourseIsInvoked_ThenReturnsCreatedResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderAllowedCoursesController sut,
        PatchProviderAllowedCourseRequestModel command,
        int ukprn,
        string larsCode)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.IsAny<PatchProviderAllowedCourseCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await sut.PatchProviderAllowedCourse(ukprn, larsCode, command);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test, MoqAutoData]
    public async Task WhenPatchProviderAllowedCourseIsInvoked_ThenVerifyMediatorIsCalled(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderAllowedCoursesController sut,
        PatchProviderAllowedCourseRequestModel command,
        int ukprn,
        string larsCode)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(
                It.Is<PatchProviderAllowedCourseCommand>(c =>
                    c.Ukprn == ukprn &&
                    c.LarsCode == larsCode &&
                    c.UserId == command.UserId &&
                    c.UserDisplayName == command.UserDisplayName &&
                    c.LastDateStarts == command.LastDateStarts),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await sut.PatchProviderAllowedCourse(ukprn, larsCode, command);

        // Assert
        mediatorMock.Verify(x => x.Send(
            It.Is<PatchProviderAllowedCourseCommand>(c =>
                c.Ukprn == ukprn &&
                c.LarsCode == larsCode &&
                c.UserId == command.UserId &&
                c.UserDisplayName == command.UserDisplayName &&
                c.LastDateStarts == command.LastDateStarts),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
