using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetLocationInformation;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(locationInformation);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(getLocationInformationResult.Name, locationInformation.Name);
            CollectionAssert.AreEqual(getLocationInformationResult.GeoPoint, locationInformation.GeoPoint);
        }
    }
}
