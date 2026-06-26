using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.RestrictedCourses.Commands.AddRestrictedCourse;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.RestrictedCoursesControllerTests;

public class RestrictedCoursesControllerPostTests
{
    [Test, MoqAutoData]
    public async Task WhenAddRestrictedCourseIsInvoked_ThenReturnsCreatedResult(
        [Greedy] RestrictedCoursesController sut,
        AddRestrictedCourseCommand command)
    {
        // Act
        var result = await sut.AddRestrictedCourse(command);

        // Assert
        Assert.That(result, Is.InstanceOf<CreatedResult>());
    }

    [Test, MoqAutoData]
    public async Task WhenAddRestrictedCourseIsInvoked_ThenVerifyMediatorIsCalled(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RestrictedCoursesController sut,
        AddRestrictedCourseCommand command)
    {
        // Act
        await sut.AddRestrictedCourse(command);

        // Assert
        mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once());
    }
}
