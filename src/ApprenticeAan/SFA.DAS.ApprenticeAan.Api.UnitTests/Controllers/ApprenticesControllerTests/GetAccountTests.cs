using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.ApprenticeAccount.Queries.GetApprenticeAccount;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.ApprenticesControllerTests;

public class GetAccountTests
{
    [Test]
    [MoqAutoData]
    public async Task GetAccount_InvokesMediator(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController sut,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {
        await sut.GetAccount(apprenticeId, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<GetApprenticeAccountQuery>(q => q.ApprenticeId == apprenticeId), cancellationToken));
    }

    [Test]
    [MoqAutoData]
    public async Task GetAccount_ApprenticeFound_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController sut,
        GetApprenticeAccountQueryResult getApprenticeAccountQueryResult,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetApprenticeAccountQuery>(q => q.ApprenticeId == apprenticeId), It.IsAny<CancellationToken>())).ReturnsAsync(getApprenticeAccountQueryResult);

        var response = await sut.GetAccount(apprenticeId, cancellationToken);

        response.As<OkObjectResult>().Should().NotBeNull();
        response.As<OkObjectResult>().Value.Should().Be(getApprenticeAccountQueryResult);
    }

    [Test]
    [MoqAutoData]
    public async Task GetAccount_ApprenticeNotFound_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController sut,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetApprenticeAccountQuery>(q => q.ApprenticeId == apprenticeId), It.IsAny<CancellationToken>())).ReturnsAsync((GetApprenticeAccountQueryResult?)null);

        var response = await sut.GetAccount(apprenticeId, cancellationToken);

        response.As<NotFoundResult>().Should().NotBeNull();
    }
}
