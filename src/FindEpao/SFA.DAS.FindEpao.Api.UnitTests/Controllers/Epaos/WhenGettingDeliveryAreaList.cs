using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindEpao.Api.Controllers;
using SFA.DAS.FindEpao.Api.Models;
using SFA.DAS.FindEpao.Application.Epaos.Queries.GetDeliveryAreaList;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindEpao.Api.UnitTests.Controllers.Epaos
{
    public class WhenGettingDeliveryAreaList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Delivery_Areas_From_Mediator(
            GetDeliveryAreaListResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EpaosController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetDeliveryAreaListQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetDeliveryAreas() as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetDeliveryAreaListResponse;
            Assert.That(model, Is.Not.Null);
            model.DeliveryAreas.Should().BeEquivalentTo(mediatorResult.DeliveryAreas);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EpaosController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetDeliveryAreaListQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetDeliveryAreas() as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}