using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.Settings.NotificationsLocations;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.EventNotificationsSettingsLocationsControllerTests;

public class EventNotificationsSettingsLocationsControllerTests
{
    [Test, MoqAutoData]
    public async Task Get_ReturnsCorrectViewModel(
        GetNotificationsLocationsQueryResult response,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EventNotificationsSettingsLocationsController sut)
    {
        mockMediator
            .Setup(m => m.Send(It.IsAny<GetNotificationsLocationsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.Get(12345, "exampleSearchTerm");

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
}