using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.Locations.Queries.GetLocationsBySearch;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.LocationsControllerTests;

public class LocationsControllerTests
{
    [Test, MoqAutoData]
    public async Task GetLocationsBySearchTask_ReturnsAddresses(
        GetLocationsBySearchQueryResult response,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LocationsController sut)
    {
        mockMediator
            .Setup(m => m.Send(It.IsAny<GetLocationsBySearchQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.GetLocationsBySearch("thisIsAQuery");

        result.As<OkObjectResult>().Should().NotBeNull();

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
}
