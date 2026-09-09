using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderAllowedCourses;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.ProviderAllowedCoursesControllerTests;

public class ProviderAllowedCoursesControllerGetTests
{
    [Test, MoqAutoData]
    public async Task WhenGetProviderAllowedCoursesIsInvoked_ThenReturnsOkResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderAllowedCoursesController sut,
        GetProviderAllowedCoursesResponse response,
        int ukprn,
        CourseType courseType)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.IsAny<GetProviderAllowedCoursesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.GetProviderAllowedCourses(ukprn, courseType, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test, MoqAutoData]
    public async Task WhenGetProviderAllowedCoursesIsInvoked_ThenMediatorIsCalled(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderAllowedCoursesController sut,
        GetProviderAllowedCoursesResponse response,
        int ukprn,
        CourseType courseType)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.Is<GetProviderAllowedCoursesQuery>(q => q.Ukprn == ukprn && q.CourseType == courseType), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        await sut.GetProviderAllowedCourses(ukprn, courseType, CancellationToken.None);

        // Assert
        mediatorMock.Verify(x => x.Send(
            It.Is<GetProviderAllowedCoursesQuery>(q => q.Ukprn == ukprn && q.CourseType == courseType), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
