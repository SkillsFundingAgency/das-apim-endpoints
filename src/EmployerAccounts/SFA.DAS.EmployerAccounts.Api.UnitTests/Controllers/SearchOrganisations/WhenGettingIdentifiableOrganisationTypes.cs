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
using SFA.DAS.EmployerAccounts.Application.Queries.GetIdentifiableOrganisationTypes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers.SearchOrganisations
{
    public class WhenGettingIdentifiableOrganisationTypes
    {
        [Test, MoqAutoData]
        public async Task Then_GetIdentifiableOrganisationTypes_From_Mediator(
            GetIdentifiableOrganisationTypesResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SearchOrganisationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetIdentifiableOrganisationTypesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetIdentifiableOrganisationTypes() as ObjectResult;

            controllerResult.Should().NotBeNull();

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as OrganisationType[];

            model.Should().NotBeNull();
            model.Should().BeEquivalentTo(mediatorResult.OrganisationTypes);
        }

        [Test, MoqAutoData]
        public async Task And_Mediator_Response_IsNull_Returns_NotFound(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SearchOrganisationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetIdentifiableOrganisationTypesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetIdentifiableOrganisationTypesResult)null);

            var controllerResult = await controller.GetIdentifiableOrganisationTypes() as NotFoundResult;

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
                    It.IsAny<GetIdentifiableOrganisationTypesQuery>(),
                    It.IsAny<CancellationToken>()))
                    .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetIdentifiableOrganisationTypes() as BadRequestResult;

            controllerResult.Should().NotBeNull();

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
