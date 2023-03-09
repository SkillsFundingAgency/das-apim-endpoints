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
using SFA.DAS.Recruit.Application.Queries.GetProviders;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Providers
{
    public class WhenGettingAllProviders
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Providers_From_Mediator(
            GetProvidersQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetProvidersQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetAllProviders() as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetProvidersListResponse;
            Assert.IsNotNull(model);
            model.Providers.Should().BeEquivalentTo(mediatorResult.Providers, options=>options
                .Excluding(x=>x.Address.AddressLine1)
                .Excluding(x => x.Address.AddressLine2)
                .Excluding(x => x.Address.AddressLine3)
                .Excluding(x => x.Address.AddressLine4)
            );

            model.Providers.First().Address.Address1.Should().Be(mediatorResult.Providers.First().Address.AddressLine1);
            model.Providers.First().Address.Address2.Should().Be(mediatorResult.Providers.First().Address.AddressLine2);
            model.Providers.First().Address.Address3.Should().Be(mediatorResult.Providers.First().Address.AddressLine3);
            model.Providers.First().Address.Address4.Should().Be(mediatorResult.Providers.First().Address.AddressLine4);
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

            var controllerResult = await controller.GetAllProviders() as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}