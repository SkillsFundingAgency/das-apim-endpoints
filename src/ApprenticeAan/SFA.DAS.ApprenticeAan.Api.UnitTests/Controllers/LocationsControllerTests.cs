using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetAddresses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers
{
    public class LocationsControllerTests
    {
        [Test]
        [MoqAutoData]
        public async Task And_MediatorCommandSuccessful_Then_ReturnOk(GetAddressesQueryResult response,
            [Frozen] Mock<IMediator> mockMediator,
            [Frozen] ILogger<LocationsController> logger)
        {
            mockMediator.Setup(m => m.Send(It.IsAny<GetAddressesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var controller = new LocationsController(logger, mockMediator.Object);

            var result = await controller.GetAddresses("thisIsAQuery") as OkObjectResult;

            result.Should().NotBeNull();

            var model = result?.Value;

            model.Should().BeEquivalentTo(response.AddressesResponse);
        }

        [Test]
        [MoqAutoData]
        public async Task And_MediatorCommandSuccessful_Then_ReturnNoResults(
            [Frozen] Mock<IMediator> mockMediator,
            [Frozen] ILogger<LocationsController> logger)
        {
            var response = new GetAddressesQueryResult(null!);
            mockMediator.Setup(m => m.Send(It.IsAny<GetAddressesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var controller = new LocationsController(logger, mockMediator.Object);

            var result = await controller.GetAddresses("thisIsAQuery");

            result.Should().NotBeNull();

            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test]
        [MoqAutoData]
        public async Task And_FromMediator_Then_Return500(
            [Frozen] Mock<IMediator> mockMediator,
            [Frozen] ILogger<LocationsController> logger,
            [Greedy] LocationsController controller)
        {
            mockMediator.Setup(m => m.Send(It.IsAny<GetAddressesQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception("Error"));

            var controllerResult = await controller.GetAddresses("thisIsAQuery") as StatusCodeResult;

            controllerResult?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}