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
using SFA.DAS.EmployerAccounts.Application.Queries.FindPublicSectorOrganisation;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers.SearchOrganisations
{
    public class WhenGettingPublicSectorOrganisations
    {
        [Test, MoqAutoData]
        public async Task Then_FindPublicSectorOrganisation_From_Mediator(
            FindPublicSectorOrganisationQuery query,
            PagedResponse<FindPublicSectorOrganisationResult> mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SearchOrganisationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<FindPublicSectorOrganisationQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetPublicSectorOrganisations(
                query.SearchTerm,
                query.PageSize,
                query.PageNumber) as ObjectResult;

            controllerResult.Should().NotBeNull();

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as FindPublicSectorOrganisationResponse;

            model.Should().NotBeNull();

            model.Data.Should().BeEquivalentTo(mediatorResult.Data);
            model.TotalPages.Should().Be(mediatorResult.TotalPages);
            model.PageNumber.Should().Be(mediatorResult.Page);
        }

        [Test, MoqAutoData]
        public async Task And_Mediator_Response_IsNull_Returns_NotFound(
             FindPublicSectorOrganisationQuery query,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SearchOrganisationController controller)
        {
            mockMediator
               .Setup(mediator => mediator.Send(
                   It.IsAny<FindPublicSectorOrganisationQuery>(),
                   It.IsAny<CancellationToken>()))
               .ReturnsAsync((PagedResponse<FindPublicSectorOrganisationResult>)null);

            var controllerResult = await controller.GetPublicSectorOrganisations(query.SearchTerm,
                query.PageSize,
                query.PageNumber) as NotFoundResult;

            controllerResult.Should().NotBeNull();

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Internal_Server_Error(
            FindPublicSectorOrganisationQuery query,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SearchOrganisationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                   query,
                    It.IsAny<CancellationToken>()))
                    .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetPublicSectorOrganisations(query.SearchTerm,
            query.PageSize,
                query.PageNumber) as BadRequestResult;

            controllerResult.Should().NotBeNull();

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
