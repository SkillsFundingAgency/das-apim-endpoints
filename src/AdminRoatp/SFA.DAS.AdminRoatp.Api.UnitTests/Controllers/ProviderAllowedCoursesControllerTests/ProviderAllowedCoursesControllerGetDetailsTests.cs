using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderAllowedCourseDetails;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.ProviderAllowedCoursesControllerTests;

public class ProviderAllowedCoursesControllerGetDetailsTests
{
    [Test, MoqAutoData]
    public async Task WhenGetProviderAllowedCourseDetailsIsInvoked_AndDetailsFound_ThenReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderAllowedCoursesController sut,
        GetProviderAllowedCourseDetailsResponse response,
        GetProviderAllowedCourseDetailsQuery query)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.IsAny<GetProviderAllowedCourseDetailsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        // Act
        var result = await sut.GetProviderAllowedCourseDetails(query.Ukprn, query.LarsCode, CancellationToken.None);
        // Assert
        result.As<OkObjectResult>().Value.Should().Be(response);
    }

    [Test, MoqAutoData]
    public async Task WhenGetProviderAllowedCourseDetailsIsInvoked_AndNullResult_ThenReturnsNoContentResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderAllowedCoursesController sut,
        GetProviderAllowedCourseDetailsQuery query)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.IsAny<GetProviderAllowedCourseDetailsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);
        // Act
        var result = await sut.GetProviderAllowedCourseDetails(query.Ukprn, query.LarsCode, CancellationToken.None);
        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}
