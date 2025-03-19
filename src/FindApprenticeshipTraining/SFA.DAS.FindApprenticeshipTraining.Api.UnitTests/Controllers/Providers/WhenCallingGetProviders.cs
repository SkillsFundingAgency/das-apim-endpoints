using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetRoatpProviders;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Providers;

public class WhenCallingGetProviders
{

    [Test, MoqAutoData]
    public async Task Then_it_Sends_Query_With_Live_False_To_Mediator(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProvidersController sut,
        CancellationToken cancellationToken
    )
    {
        await sut.GetProviders(false, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(
                It.Is<GetRoatpProvidersQuery>(a => a.Live.Equals(false)),
                It.IsAny<CancellationToken>()
            )
        );
    }

    [Test, MoqAutoData]
    public async Task Then_it_Sends_Query_With_Live_True_To_Mediator(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProvidersController sut,
        CancellationToken cancellationToken
    )
    {
        await sut.GetProviders(true, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(
                It.Is<GetRoatpProvidersQuery>(a => a.Live.Equals(true)),
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
                It.Is<GetRoatpProvidersQuery>(a => a.Live.Equals(false)), 
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(queryResult);

        var result = await sut.GetProviders(false, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }
}
