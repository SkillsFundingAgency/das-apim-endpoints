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
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.CommitmentPayment.Queries.GetDataLockEvents;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.CommitmentPayment
{
    public class WhenGettingDataLockStatuses
    {
        [Test, MoqAutoData]
        public async Task Then_Get_DataLockEvents_From_Mediator(
            long sinceEventId,
            DateTime? sinceTime,
            string employerAccountId,
            long ukprn,
            int page,
            GetDataLockStatusesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DataLockController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetDataLockStatuesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetDataLockStatuses(sinceEventId, sinceTime, employerAccountId, ukprn, page) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetDataLockStatusListResponse;
            Assert.That(model, Is.Not.Null);
            model.Should().BeOfType<GetDataLockStatusListResponse>();
        }

        [Test, MoqAutoData]
        public async Task Then_Get_TotalNumberOfItems_From_Mediator(
            long sinceEventId,
            DateTime? sinceTime,
            string employerAccountId,
            long ukprn,
            int page,
            GetDataLockStatusesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DataLockController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetDataLockStatuesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetDataLockStatuses(sinceEventId, sinceTime, employerAccountId, ukprn, page) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetDataLockStatusListResponse;
            Assert.That(model, Is.Not.Null);
            model.TotalNumberOfPages.Should().Be(mediatorResult.PagedDataLockEvent.TotalNumberOfPages);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            long sinceEventId,
            DateTime? sinceTime,
            string employerAccountId,
            long ukprn,
            int page,
            GetDataLockStatusesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DataLockController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetDataLockStatuesQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetDataLockStatuses(sinceEventId, sinceTime, employerAccountId, ukprn, page) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}