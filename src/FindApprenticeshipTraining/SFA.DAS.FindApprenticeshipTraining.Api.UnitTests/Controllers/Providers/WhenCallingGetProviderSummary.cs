using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetProviderSummary;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Providers;

public class WhenCallingGetProviderSummary
{

    [Test, MoqAutoData]
    public async Task Then_it_Sends_Query_To_Mediator(
        int ukprn,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProvidersController sut,
        CancellationToken cancellationToken
    )
    {
        await sut.GetProviderSummary(ukprn, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(
                It.Is<GetProviderSummaryQuery>(a => a.Ukprn.Equals(ukprn)),
                It.IsAny<CancellationToken>()
            )
        );
    }

    [Test, MoqAutoData]
    public async Task Then_it_Return_ExpectedResponse_From_Mediator(
        int ukprn,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProvidersController sut,
        GetProviderSummaryQueryResult queryResult,
        CancellationToken cancellationToken
    )
    {
        mediatorMock.Setup(m =>
            m.Send(
                It.Is<GetProviderSummaryQuery>(a => a.Ukprn.Equals(ukprn)),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(queryResult);

        var result = await sut.GetProviderSummary(ukprn, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }
}
