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
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.Reservations.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Reservations
{
    public class WhenGettingReservationsStatus
    {
        [Test, MoqAutoData]
        public async Task Then_Calls_AccountReservationQuery_From_Mediator(
            GetAccountReservationsStatusQueryResult mediatorResult,
            long accountId,
            long? transferSenderId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ReservationsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAccountReservationsStatusQuery>(p=>p.AccountId == accountId && p.TransferSenderId == transferSenderId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetStatus(accountId, transferSenderId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAccountReservationsStatusQueryResult;
            Assert.That(model, Is.Not.Null);
            model.Should().Be(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAccountReservationsStatusQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetAll() as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}