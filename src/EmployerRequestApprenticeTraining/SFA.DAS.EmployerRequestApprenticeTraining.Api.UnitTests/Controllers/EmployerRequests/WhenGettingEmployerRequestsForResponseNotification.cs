using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequestsForResponseNotification;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequests
{
    public class WhenGettingEmployerRequestsForResponseNotification
    {
        [Test, MoqAutoData]
        public async Task Then_The_EmployerRequestsForNotification_Are_Returned_From_Mediator(
            long accountId,
            GetEmployerRequestsForResponseNotificationResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerRequestsController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetEmployerRequestsForResponseNotificationQuery>(), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetEmployerRequestsForResponseNotification() as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(queryResult.EmployerRequests);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            long accountId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetEmployerRequestsForResponseNotificationQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetEmployerRequestsForResponseNotification() as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
