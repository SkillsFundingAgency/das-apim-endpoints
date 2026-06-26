using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.RestrictedCourses.Queries.GetAllRestrictedCourses;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.RestrictedCoursesControllerTests;

public class RestrictedCoursesControllerGetTests
{
    [Test, MoqAutoData]
    public async Task WhenGetRestrictedCoursesIsInvoked_ThenReturnsOkResult(
        [Frozen] Mock<IMediator> _mediatorMock,
        [Greedy] RestrictedCoursesController sut,
        GetAllRestrictedCoursesResponse expected)
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllRestrictedCoursesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await sut.GetRestrictedCourses(true);

        // Assert
        result.As<OkObjectResult>().Value.Should().Be(expected);
    }
}
