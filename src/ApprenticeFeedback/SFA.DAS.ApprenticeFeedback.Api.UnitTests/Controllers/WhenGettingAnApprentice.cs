using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.UnitTests.Controllers
{
    public class WhenGettingAnApprentice
    {
        [Test, MoqAutoData]
        public async Task Then_GetsApprenticeFromMediator(
                Guid apprenticeId,
                GetApprenticeResult mediatorResult,
                [Frozen] Mock<IMediator> mockMediator,
                [Greedy] ApprenticeController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApprenticeQuery>(x => x.ApprenticeId == apprenticeId),
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
        public async Task And_NoApprenticeIsReturnedFromMediator_Then_ReturnNotFound(
            Guid apprenticeId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApprenticeController controller)
        {
            mockMediator.Setup(m => m.Send(It.IsAny<GetApprenticeQuery>(), CancellationToken.None))
                .ReturnsAsync((GetApprenticeResult)null);

            var controllerResult = await controller.Get(apprenticeId) as NotFoundResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorThrowsException_Then_ReturnBadRequest(
            Guid apprenticeId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApprenticeController controller)
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
