using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderAvailableCourses;
using SFA.DAS.SharedOuterApi.Types.InnerApi;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.ProviderAvailableCoursesControllerTests;

public class ProviderAvailableCoursesControllerGetTests
{
    [Test, MoqAutoData]
    public async Task WhenGetProviderAvailableCoursesIsInvoked_ThenReturnsOkResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderAvailableCoursesController sut,
        GetProviderAvailableCoursesQueryResult response,
        int ukprn,
        CourseType courseType)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.IsAny<GetProviderAvailableCoursesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.GetProviderAvailableCourses(ukprn, courseType);

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test, MoqAutoData]
    public async Task WhenGetProviderAvailableCoursesIsInvoked_ThenMediatorIsCalled(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderAvailableCoursesController sut,
        GetProviderAvailableCoursesQueryResult response,
        int ukprn,
        CourseType courseType)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.Is<GetProviderAvailableCoursesQuery>(q => q.Ukprn == ukprn && q.CourseType == courseType), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        await sut.GetProviderAvailableCourses(ukprn, courseType);

        // Assert
        mediatorMock
            .Verify(x => x.Send(It.Is<GetProviderAvailableCoursesQuery>(q => q.Ukprn == ukprn && q.CourseType == courseType), It.IsAny<CancellationToken>()), Times.Once());
    }
}
