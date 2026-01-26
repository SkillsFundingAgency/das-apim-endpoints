using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.Providers.Queries;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Providers
{
    public class WhenGettingAllProviders
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Providers_From_Mediator(
            GetProvidersResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetProvidersQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetAll() as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetProvidersListResponse;
            Assert.That(model, Is.Not.Null);
            model.Providers.Should().BeEquivalentTo(mediatorResult.Providers);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetProvidersQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetAll() as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}