using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetProviderSummary;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Providers;

public class ProvidersControllerGetProviderSummaryTests
{

    [Test, MoqAutoData]
    public async Task GetProviderSummary_SendsQueryToMediator(
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
    public async Task GetProviderSummary_ReturnsExpectedResponseFromMediator(
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

    [Test, MoqAutoData]
    public async Task GetProviderSummary_ResultNull_ReturnsNotFound(
        int ukprn,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProvidersController sut,
        CancellationToken cancellationToken)
    {
        mediator
            .Setup(m => m.Send(It.Is<GetProviderSummaryQuery>(q => q.Ukprn == ukprn), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetProviderSummaryQueryResult)null);

        var actionResult = await sut.GetProviderSummary(ukprn, cancellationToken);

        actionResult.Should().BeOfType<NotFoundResult>();
    }
}
