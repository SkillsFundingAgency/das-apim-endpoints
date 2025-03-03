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
using SFA.DAS.EmployerAccounts.Application.Models;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAgreementTemplates;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers.EmployerAgreementTemplates;
public class WhenGettingEmployerAgreementTemplates
{
    [Test, MoqAutoData]
    public async Task Then_GetEmployerAgreementTemplates_From_Mediator(
          GetEmployerAgreementTemplatesResponse mediatorResult,
          [Frozen] Mock<IMediator> mockMediator,
          [Greedy] EmployerAgreementTemplatesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetEmployerAgreementTemplatesQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.Index() as ObjectResult;

        controllerResult.Should().NotBeNull();

        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as List<EmployerAgreementTemplate>;

        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(mediatorResult.EmployerAgreementTemplates);
    }

    [Test, MoqAutoData]
    public async Task And_Mediator_Response_IsNull_Returns_NotFound(
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAgreementTemplatesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetEmployerAgreementTemplatesQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetEmployerAgreementTemplatesResponse)null);

        var controllerResult = await controller.Index() as NotFoundResult;

        controllerResult.Should().NotBeNull();

        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Internal_Server_Error(
       [Frozen] Mock<IMediator> mockMediator,
       [Greedy] EmployerAgreementTemplatesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetEmployerAgreementTemplatesQuery>(),
                It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

        var controllerResult = await controller.Index() as BadRequestResult;

        controllerResult.Should().NotBeNull();

        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}
