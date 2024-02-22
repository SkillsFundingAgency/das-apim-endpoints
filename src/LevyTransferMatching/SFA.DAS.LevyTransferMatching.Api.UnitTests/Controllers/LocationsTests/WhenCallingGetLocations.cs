using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetLocations;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.LocationsTests
{
    public class WhenCallingGetLocations
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Locations_From_Mediator(
            GetLocationsResult getLocationsResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LocationController locationController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.IsAny<GetLocationsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getLocationsResult);

            var controllerResult = await locationController.GetLocations("test");
            var okObjectResult = controllerResult as OkObjectResult;
            var locations = okObjectResult.Value as LocationsDto;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.That(okObjectResult, Is.Not.Null);
            Assert.That(locations, Is.Not.Null);
            Assert.That(okObjectResult.StatusCode,Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(getLocationsResult.Locations.Count(), Is.EqualTo(locations.Names.Count()));
        }
    }
}
