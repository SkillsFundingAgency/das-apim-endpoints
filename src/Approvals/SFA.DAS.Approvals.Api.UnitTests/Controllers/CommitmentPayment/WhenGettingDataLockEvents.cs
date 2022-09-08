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
using SFA.DAS.Approvals.Application.CommitmentPayment.Queries.GetDataLockEvents;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderEvent;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.CommitmentPayment
{
    public class WhenGettingDataLockEvents
    {
        [Test, MoqAutoData]
        public async Task Then_Get_DataLockEvents_From_Mediator(
            long sinceEventId,
            DateTime? sinceTime,
            string employerAccountId,
            long ukprn,
            int page,
            GetDataLockEventsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DataLockController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetDataLockEventsQuery>(x => x.EmployerAccountId == employerAccountId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetDataLockEvents(sinceEventId, sinceTime, employerAccountId, ukprn, page) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as PageOfResults<GetDataLockEventsResponse>;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(mediatorResult.PagedDataLockEvent);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            long sinceEventId,
            DateTime? sinceTime,
            string employerAccountId,
            long ukprn,
            int page,
            GetDataLockEventsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DataLockController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetDataLockEventsQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetDataLockEvents(sinceEventId, sinceTime, employerAccountId, ukprn, page) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}