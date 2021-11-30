﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetGeocode;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Locations
{
    public class WhenGettingGeoPoint
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_GeoPoint_From_Mediator(
            GetGeoPointQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LocationsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetGeoPointQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetGeopoint("AB1 2CD") as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetGeoPointResponse;
            Assert.IsNotNull(model);
            model.GeoPoint.Should().BeEquivalentTo(mediatorResult.GetPointResponse.GeoPoint);
        }
    }
}
