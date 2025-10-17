using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetRemovedReasons;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.RemovedReasonsControllerTests;
public class RemovedReasonsControllerGetAllRemovedReasonsTests
{
    [Test, MoqAutoData]

    public async Task GetAllRemovedReasons_ReturnsSuccessfulResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RemovedReasonsController sut,
        GetRemovedReasonsQuery query,
        GetRemovedReasonsResponse expectedResponse)
    {
        // Arrange
        mediatorMock.Setup(m => m.Send(It.IsAny<GetRemovedReasonsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

        // Act
        var result = await sut.GetAllRemovedReasons(CancellationToken.None);
        var response = (OkObjectResult)result;

        // Assert
        mediatorMock.Verify(m => m.Send(It.IsAny<GetRemovedReasonsQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        response.Value.Should().BeEquivalentTo(expectedResponse);
    }
}