using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.NotificationsLocations.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.NotificationsLocations;

public class NotificationsLocationsControllerTests
{
    [Test, MoqAutoData]
    public async Task Get_ReturnsCorrectViewModel(
        GetNotificationsLocationsQueryResult response,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] NotificationsLocationsController sut)
    {
        mockMediator
            .Setup(m => m.Send(It.IsAny<GetNotificationsLocationsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.Get("exampleSearchTerm");

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
}