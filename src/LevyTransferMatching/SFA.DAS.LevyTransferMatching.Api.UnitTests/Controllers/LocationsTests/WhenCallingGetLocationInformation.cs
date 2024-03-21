using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetLocationInformation;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.LocationsTests
{
    public class WhenCallingGetLocationInformation
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Locations_From_Mediator(
            GetLocationInformationResult getLocationInformationResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LocationController locationController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.IsAny<GetLocationInformationQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getLocationInformationResult);

            var controllerResult = await locationController.GetLocationInformation("test");
            var okObjectResult = controllerResult as OkObjectResult;
            var locationInformation = okObjectResult.Value as LocationInformationDto;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.That(okObjectResult, Is.Not.Null);
            Assert.That(locationInformation, Is.Not.Null);
            Assert.That(okObjectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(getLocationInformationResult.Name, Is.EqualTo(locationInformation.Name));
            locationInformation.GeoPoint.Should().BeEquivalentTo(getLocationInformationResult.GeoPoint);
        }
    }
}
