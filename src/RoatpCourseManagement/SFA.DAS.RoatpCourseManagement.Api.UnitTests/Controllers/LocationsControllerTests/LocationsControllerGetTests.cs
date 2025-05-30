using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAddresses;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.LocationsControllerTests;

[TestFixture]
public class LocationsControllerGetTests
{
    private Mock<IMediator> _mediator;
    private LocationsController _sut;
    [SetUp]
    public void Before_Each_Test()
    {
        _mediator = new Mock<IMediator>();
        _sut = new LocationsController(_mediator.Object);
    }

    [Test]
    public async Task GetAddresses_ReturnsExpectedResponse()
    {
        var searchTerm = "search";
        var expectedResponse = new GetAddressesQueryResult();
        _mediator.Setup(m => m.Send(It.IsAny<GetAddressesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

        var result = await _sut.GetAddresses(searchTerm);

        var response = (OkObjectResult)result;

        response.Should().NotBeNull();
        response.Value.Should().Be(expectedResponse);
    }
}