using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Apprentices.Queries.GetApprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.ApprenticesControllerTests;

public class GetApprenticeTests
{
    [Test, MoqAutoData]
    public async Task GetApprentice_InvokesMediator(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController sut,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {
        await sut.GetApprentice(apprenticeId, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<GetApprenticeQuery>(q => q.ApprenticeId == apprenticeId), cancellationToken));
    }

    [Test, MoqAutoData]
    public async Task GetApprentice_RecordNotFound_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController sut,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetApprenticeQuery>(q => q.ApprenticeId == apprenticeId), cancellationToken)).ReturnsAsync(() => null);

        var result = await sut.GetApprentice(apprenticeId, cancellationToken);

        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task GetApprentice_RecordFound_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController sut,
        GetApprenticeQueryResult expectedResult,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetApprenticeQuery>(q => q.ApprenticeId == apprenticeId), cancellationToken)).ReturnsAsync(expectedResult);

        var result = await sut.GetApprentice(apprenticeId, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.As<GetApprenticeQueryResult?>().Should().Be(expectedResult);
    }
}
