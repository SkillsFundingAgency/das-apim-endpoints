using System;
using System.Linq;
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
using SFA.DAS.Recruit.Application.Queries.GetProvider;
using SFA.DAS.Recruit.Application.Queries.GetProviders;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Providers
{
    public class WhenGettingProvider
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Provider_From_Mediator(
            GetProviderQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetProviderQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetProvider(mediatorResult.Ukprn) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult?.Value as GetTrainingProviderResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Null_Then_Returns_NotFound_Request(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetProviderQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetProviderQueryResult)null);

            var controllerResult = await controller.GetProvider(It.IsAny<int>()) as NotFoundResult;

            controllerResult?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetProviderQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetProvider(It.IsAny<int>()) as BadRequestResult;

            controllerResult?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}