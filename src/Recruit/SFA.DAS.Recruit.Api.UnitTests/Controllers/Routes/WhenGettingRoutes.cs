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
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.Routes;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Routes
{
    public class WhenGettingRoutes
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Routes_From_Mediator(
            GetRoutesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]RoutesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetRoutesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetRoutes() as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetRoutesResponse;
            Assert.That(model, Is.Not.Null);

            foreach (var sector in model.Routes)
            {
                mediatorResult.Routes.Should().Contain(c => c.Name.Equals(sector.Route));
                mediatorResult.Routes.Should().Contain(c => c.Id.Equals(sector.Id));
            }
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_InternalServerError(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]RoutesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetRoutesQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetRoutes() as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}