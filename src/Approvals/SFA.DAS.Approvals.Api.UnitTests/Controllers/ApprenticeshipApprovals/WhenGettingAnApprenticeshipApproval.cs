using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.ApprenticeshipApprovals.Query;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.ApprenticeshipApprovals;

public class WhenGettingAnApprenticeshipApproval
{
    [Test, MoqAutoData]
    public async Task Then_Gets_ApprovalDetails_From_Mediator(
        long apprenticeId,
        long accountId,
        GetApprenticeshipApprovalResponse mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticeshipApprovalsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetApprenticeshipApprovalQuery>(x=>x.ApprenticeshipId == apprenticeId && x.EmployerAccountId == accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetApprenticeshipApproval(accountId, apprenticeId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetApprenticeshipApprovalResponse;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Then_No_ApprovalDetails_Are_Found(
        long apprenticeId,
        long accountId,
        GetApprenticeshipApprovalResponse mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticeshipApprovalsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetApprenticeshipApprovalQuery>(x => x.ApprenticeshipId == apprenticeId && x.EmployerAccountId == accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var controllerResult = await controller.GetApprenticeshipApproval(accountId, apprenticeId) as NotFoundResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test, MoqAutoData]
    public async Task And_UnauthorisedException_Then_Returns_ForBidden(
        long apprenticeId,
        long accountId,
        GetApprenticeshipApprovalResponse mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticeshipApprovalsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetApprenticeshipApprovalQuery>(x => x.ApprenticeshipId == apprenticeId && x.EmployerAccountId == accountId),
                It.IsAny<CancellationToken>()))
            .Throws<UnauthorizedAccessException>();

        var controllerResult = await controller.GetApprenticeshipApproval(accountId, apprenticeId) as StatusCodeResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
    }

    [Test, MoqAutoData]
    public async Task And_UnhandledException_Then_Returns_InternalServerError(
        long apprenticeId,
        long accountId,
        GetApprenticeshipApprovalResponse mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticeshipApprovalsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetApprenticeshipApprovalQuery>(x => x.ApprenticeshipId == apprenticeId && x.EmployerAccountId == accountId),
                It.IsAny<CancellationToken>()))
            .Throws<ApplicationException>();

        var controllerResult = await controller.GetApprenticeshipApproval(accountId, apprenticeId) as StatusCodeResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
