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
using SFA.DAS.EmployerAccounts.Api.Controllers;
using SFA.DAS.EmployerAccounts.Api.Models;
using SFA.DAS.EmployerAccounts.Application.Queries.GetCharity;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers.SearchOrganisations
{
    public class WhenGettingCharity
    {
        [Test, MoqAutoData]
        public async Task Then_GetCharity_From_Mediator(
            int registrationNumber,
            GetCharityResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SearchOrganisationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCharityQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetCharity(registrationNumber) as ObjectResult;

            controllerResult.Should().NotBeNull();

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetCharityResponse;

            model.Should().NotBeNull();

            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Mediator_Response_IsNull_Returns_NotFound(
            int registrationNumber,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SearchOrganisationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCharityQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetCharityResult)null);

            var controllerResult = await controller.GetCharity(registrationNumber) as NotFoundResult;

            controllerResult.Should().NotBeNull();

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Internal_Server_Error(
            int registrationNumber,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SearchOrganisationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCharityQuery>(),
                    It.IsAny<CancellationToken>()))
                    .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetCharity(registrationNumber) as BadRequestResult;

            controllerResult.Should().NotBeNull();

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
