using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Commands.UpdateProviderAllowedCourse;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.ProviderAllowedCoursesControllerTests;

public class ProviderAllowedCoursesControllerPutTests
{
    [Test, MoqAutoData]
    public async Task WhenPutProviderAllowedCourseIsInvoked_ThenReturnsNoContentResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderAllowedCoursesController sut,
        PutProviderAllowedCourseModel command,
        int ukprn,
        string larsCode)
    {
        CancellationToken cancellationToken = new();
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.IsAny<UpdateProviderAllowedCourseCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await sut.UpdateProviderAllowedCourse(ukprn, larsCode, command, cancellationToken);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test, MoqAutoData]
    public async Task WhenPutProviderAllowedCourseIsInvoked_ThenVerifyMediatorIsCalled(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderAllowedCoursesController sut,
        PutProviderAllowedCourseModel command,
        int ukprn,
        string larsCode)
    {
        CancellationToken cancellationToken = new();
        // Arrange
        mediatorMock
            .Setup(x => x.Send(
                It.Is<UpdateProviderAllowedCourseCommand>(c =>
                    c.Ukprn == ukprn &&
                    c.LarsCode == larsCode &&
                    c.RequestModel.UserId == command.UserId &&
                    c.RequestModel.UserDisplayName == command.UserDisplayName &&
                    c.RequestModel.LastDateStarts == command.LastDateStarts),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await sut.UpdateProviderAllowedCourse(ukprn, larsCode, command, cancellationToken);

        // Assert
        mediatorMock.Verify(x => x.Send(
            It.Is<UpdateProviderAllowedCourseCommand>(c =>
                c.Ukprn == ukprn &&
                c.LarsCode == larsCode &&
                c.RequestModel.UserId == command.UserId &&
                c.RequestModel.UserDisplayName == command.UserDisplayName &&
                c.RequestModel.LastDateStarts == command.LastDateStarts),
            cancellationToken), Times.Once);
    }
}
