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
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetProvider;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Providers
{
    public class WhenGettingProvider
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Provider_From_Mediator(
            int ukprn,
            GetProviderQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetProviderQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetProvider(ukprn) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult?.Value as GetProviderResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(mediatorResult.Provider, options=>options
                .Excluding(x=>x.Address.AddressLine1)
                .Excluding(x => x.Address.AddressLine2)
                .Excluding(x => x.Address.AddressLine3)
                .Excluding(x => x.Address.AddressLine4)
            );

            model?.Address.Address1.Should().Be(mediatorResult.Provider.Address.AddressLine1);
            model?.Address.Address2.Should().Be(mediatorResult.Provider.Address.AddressLine2);
            model?.Address.Address3.Should().Be(mediatorResult.Provider.Address.AddressLine3);
            model?.Address.Address4.Should().Be(mediatorResult.Provider.Address.AddressLine4);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int ukprn,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetProviderQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetProvider(ukprn) as BadRequestResult;

            controllerResult?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Test, MoqAutoData]
        public async Task And_NotFound_Then_Returns_Not_Request(
            int ukprn,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetProviderQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetProviderQueryResult)null);

            var controllerResult = await controller.GetProvider(ukprn) as NotFoundResult;

            Assert.IsNotNull(controllerResult);
            controllerResult?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}