using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.ApprenticeAccount.Queries.GetApprenticeAccount;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers
{
    public class MyApprenticeshipControllerTests
    {
        [Test]
        [MoqAutoData]
        public async Task GetMyApprenticeship_InvokesMediator(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy]MyApprenticeshipController sut,
            Guid apprenticeId,
            CancellationToken cancellationToken)
        {
            await sut.GetMyApprenticeship(apprenticeId, cancellationToken);
            mediatorMock.Verify(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == apprenticeId), cancellationToken));
        }
        
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
        }
        
        [Test]
        [MoqAutoData]
        public async Task GetMyApprenticeship_ApprenticeNotFound_ReturnsNotFoundResponse(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] MyApprenticeshipController sut,
            Guid apprenticeId,
            CancellationToken cancellationToken)
        {
            mediatorMock.Setup(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == apprenticeId), It.IsAny<CancellationToken>())).ReturnsAsync((GetMyApprenticeshipQueryResult?)null);
        
            var response = await sut.GetMyApprenticeship(apprenticeId, cancellationToken);
        
            response.As<NotFoundResult>().Should().NotBeNull();
        }
    }
}
