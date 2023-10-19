using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.StagedApprentices.Queries.GetStagedApprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers;

public class StagedApprenticesControllerTests
{
    [Test, MoqAutoData]
    public async Task GetStagedApprentice_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] StagedApprenticesController sut,
        GetStagedApprenticeQueryResult result,
        GetStagedApprenticeQuery query,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(s => s.Send(query, cancellationToken)).ReturnsAsync(result);

        await sut.GetStagedApprentice(query, cancellationToken);

        mediatorMock.Verify(s => s.Send(query, cancellationToken));
    }

    [Test, MoqAutoData]
    public async Task GetStagedApprentice_DataFound_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] StagedApprenticesController sut,
        GetStagedApprenticeQueryResult result,
        GetStagedApprenticeQuery query,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(s => s.Send(It.IsAny<GetStagedApprenticeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actualResult = await sut.GetStagedApprentice(query, cancellationToken);

        actualResult.As<OkObjectResult>().Value.As<GetStagedApprenticeQueryResult>().Should().Be(result);
    }

    [Test, MoqAutoData]
    public async Task GetStagedApprentice_DataNotFound_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] StagedApprenticesController sut,
        GetStagedApprenticeQuery query,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(s => s.Send(It.IsAny<GetStagedApprenticeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null!);

        var actualResult = await sut.GetStagedApprentice(query, cancellationToken);

        actualResult.As<NotFoundResult>().Should().NotBeNull();
    }
}
