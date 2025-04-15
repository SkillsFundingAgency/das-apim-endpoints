using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetRoatpProviders;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Providers;

public class WhenCallingGetProviders
{

    [Test, MoqAutoData]
    public async Task Then_it_Sends_Query_To_Mediator(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProvidersController sut,
        CancellationToken cancellationToken
    )
    {
        await sut.GetProviders(cancellationToken);

        mediatorMock.Verify(
            m => m.Send(
                It.IsAny<GetRoatpProvidersQuery>(),
                It.IsAny<CancellationToken>()
            )
        );
    }

    [Test, MoqAutoData]
    public async Task Then_it_Return_ExpectedResponse_From_Mediator(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProvidersController sut,
        GetRoatpProvidersQueryResult queryResult,
        CancellationToken cancellationToken
    )
    {
        mediatorMock.Setup(m =>
            m.Send(
                It.IsAny<GetRoatpProvidersQuery>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(queryResult);

        var result = await sut.GetProviders(cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }
}
