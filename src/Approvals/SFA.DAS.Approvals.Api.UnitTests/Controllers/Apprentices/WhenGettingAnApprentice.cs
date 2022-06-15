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
using SFA.DAS.Approvals.Application.Apprentices.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices
{
    public class WhenGettingAnApprentice
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Apprentice_From_Mediator(
            Guid apprenticeId,
            GetApprenticeResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApprenticesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApprenticeQuery>(x=>x.ApprenticeId == apprenticeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.Get(apprenticeId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetApprenticeResult;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Then_No_Apprentice_Is_Returned_From_Mediator(
            Guid apprenticeId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApprenticesController controller)
        {
            var controllerResult = await controller.Get(apprenticeId) as NotFoundResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            Guid apprenticeId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApprenticesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetApprenticeQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(apprenticeId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}