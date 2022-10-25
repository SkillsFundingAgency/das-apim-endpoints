﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Api.Controllers;
using SFA.DAS.EmployerAccounts.Api.Models.Reservations;
using SFA.DAS.EmployerAccounts.Application.Queries.GetReservations;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers.Reservations
{
    public class WhenGettingAllReservations
    {
        [Test]
        [MoqAutoData]
        public async Task Then_Gets_Reservations_From_Mediator(
            string accountId,
            GetReservationsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ReservationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetReservationsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetReservations(accountId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetReservationsResponse;
            Assert.IsNotNull(model);
            model.Reservations.Should().BeEquivalentTo(mediatorResult.Reservations);
        }

        [Test]
        [MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            string accountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ReservationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetReservationsQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetReservations(accountId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}