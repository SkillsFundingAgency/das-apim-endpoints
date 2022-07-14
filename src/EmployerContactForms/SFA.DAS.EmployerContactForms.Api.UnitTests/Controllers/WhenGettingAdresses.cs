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
using SFA.DAS.EmployerContactForms.Api.Controllers;
using SFA.DAS.EmployerContactForms.Application.Queries.GetAddresses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerContactForms.Api.UnitTests.Controllers
{
    public class WhenGettingAdresses
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Addresses_From_Mediator(
            GetAddressesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LocationsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAddressesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.Index("AB1 2CD") as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAddressesListResponse;
            Assert.IsNotNull(model);
            model.Addresses.Should().BeEquivalentTo(mediatorResult.AddressesResponse.Addresses.Select(item => item));
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Internal_Server_Error(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LocationsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAddressesQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Index("AB1 2CD") as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}