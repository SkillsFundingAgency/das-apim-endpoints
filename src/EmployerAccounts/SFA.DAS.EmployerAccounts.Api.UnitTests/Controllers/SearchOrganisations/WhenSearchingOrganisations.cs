using System;
using System.Collections.Generic;
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
using SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers.SearchOrganisations
{
    public class WhenSearchingOrganisations
    {
        [Test, MoqAutoData]
        public async Task Then_SearchesOrganisations_From_Mediator(
          SearchOrganisationsResult mediatorResult,
          [Frozen] Mock<IMediator> mockMediator,
          [Greedy] SearchOrganisationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<SearchOrganisationsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.SearchOrganisations("AB1 2CD") as ObjectResult;

            controllerResult.Should().NotBeNull();

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as IEnumerable<SearchOrganisationsResponse.Organisation>;

            model.Should().NotBeNull();

            model.Should().BeEquivalentTo(mediatorResult.Organisations);
        }

        [Test, MoqAutoData]
        public async Task And_Mediator_Response_IsNull_Returns_NotFound(
         [Frozen] Mock<IMediator> mockMediator,
         [Greedy] SearchOrganisationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<SearchOrganisationsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((SearchOrganisationsResult)null);

            var controllerResult = await controller.SearchOrganisations("AB1 2CD") as NotFoundResult;

            controllerResult.Should().NotBeNull();

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Internal_Server_Error(
          [Frozen] Mock<IMediator> mockMediator,
          [Greedy] SearchOrganisationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<SearchOrganisationsQuery>(),
                    It.IsAny<CancellationToken>()))
                    .Throws<InvalidOperationException>();

            var controllerResult = await controller.SearchOrganisations("AB1 2CD") as BadRequestResult;

            controllerResult.Should().NotBeNull();

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
