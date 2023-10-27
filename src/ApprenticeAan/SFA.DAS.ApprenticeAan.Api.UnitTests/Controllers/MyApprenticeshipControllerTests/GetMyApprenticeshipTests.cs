using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.MyApprenticeshipControllerTests;

public class GetMyApprenticeshipTests
{
    [Test]
    [MoqAutoData]
    public async Task GetMyApprenticeship_ApprenticeFound_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipController sut,
        GetMyApprenticeshipQueryResult result,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == apprenticeId), It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var response = await sut.GetMyApprenticeship(apprenticeId, cancellationToken);

        response.As<OkObjectResult>().Should().NotBeNull();
        response.As<OkObjectResult>().Value.Should().Be(result);
        mediatorMock.Verify(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == apprenticeId), cancellationToken));
    }

    [Test]
    [MoqAutoData]
    public async Task GetMyApprenticeship_MyApprenticeshipNotFound_ReturnsNotFound(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipController sut,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == apprenticeId), It.IsAny<CancellationToken>())).ReturnsAsync((GetMyApprenticeshipQueryResult)null!);
        var response = await sut.GetMyApprenticeship(apprenticeId, cancellationToken);
        response.As<NotFoundResult>().Should().NotBeNull();
        mediatorMock.Verify(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == apprenticeId), cancellationToken));
    }
}
