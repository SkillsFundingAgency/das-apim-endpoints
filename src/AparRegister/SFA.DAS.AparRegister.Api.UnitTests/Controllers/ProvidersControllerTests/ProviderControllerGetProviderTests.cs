using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AparRegister.Api.Controllers;
using SFA.DAS.AparRegister.Api.Models;
using SFA.DAS.AparRegister.Application.Queries.ProviderRegister;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AparRegister.Api.UnitTests.Controllers.ProvidersControllerTests;

public class ProviderControllerGetProviderTests
{
    [Test, MoqAutoData]
    public async Task GetProviders_InvokesMediator(
        GetProvidersQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProvidersController controller,
        CancellationToken cancellationToken)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetProvidersQuery>(), cancellationToken)).ReturnsAsync(mediatorResponse);

        await controller.GetProviders(cancellationToken);

        mediator.Verify(mediator => mediator.Send(It.IsAny<GetProvidersQuery>(), cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task GetProviders_ReturnsOkResult(
        GetProvidersQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProvidersController controller,
        CancellationToken cancellationToken)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetProvidersQuery>(), cancellationToken)).ReturnsAsync(mediatorResponse);

        IActionResult actual = await controller.GetProviders(cancellationToken);

        using (new AssertionScope())
        {
            actual.As<OkObjectResult>().Should().NotBeNull();
            actual.As<OkObjectResult>().Value.As<ProvidersApiResponse>().Should().NotBeNull();
            actual.As<OkObjectResult>().Value.As<ProvidersApiResponse>().Providers.Should().HaveCount(mediatorResponse.RegisteredProviders.Count());
        }
    }
}
