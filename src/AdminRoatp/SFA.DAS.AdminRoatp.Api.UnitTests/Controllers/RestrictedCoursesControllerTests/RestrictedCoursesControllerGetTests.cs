using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetAllRestrictedCourses;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.RestrictedCoursesControllerTests;

public class RestrictedCoursesControllerTests
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
