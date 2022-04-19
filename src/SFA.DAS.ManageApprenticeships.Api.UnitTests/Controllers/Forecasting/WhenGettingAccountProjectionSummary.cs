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
using SFA.DAS.ManageApprenticeships.Api.Controllers;
using SFA.DAS.ManageApprenticeships.Api.Models;
using SFA.DAS.ManageApprenticeships.Application.Queries.GetAccountProjectionSummary;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ManageApprenticeships.Api.UnitTests.Controllers.Providers
{
    public class WhenGettingAccountProjectionSummary
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Summary_From_Mediator(
            GetAccountProjectionSummaryQueryResult mediatorResult,
            long accountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProjectionsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAccountProjectionSummaryQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetAccountProjectionSummary(accountId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAccountProjectionSummaryResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            long accountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProjectionsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAccountProjectionSummaryQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetAccountProjectionSummary(accountId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}