using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Commands.ProcessApprenticeshipApproval;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.ApprenticeshipApprovals;

public class WhenProcessingAnApprenticeshipApproval
{
    [Test, MoqAutoData]
    public async Task Then_Sends_ApprovalDetails_To_Mediator(
        Guid approvalRequestId,
        long apprenticeId,
        long accountId,
        ProcessApprenticeshipApprovalRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticeshipApprovalsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<ProcessApprenticeshipApprovalCommand>(),
                It.IsAny<CancellationToken>()));

        var controllerResult = await controller.PostApprenticeshipApproval(accountId, apprenticeId, approvalRequestId, request) as OkResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        mockMediator.Verify(x=>x.Send(
            It.Is<ProcessApprenticeshipApprovalCommand>(c =>
                c.ApprenticeshipId == apprenticeId &&
                c.ApprovalRequestId == approvalRequestId &&
                c.ApplyChanges == request.ApplyChanges &&
                c.UserInfo.UserId == request.UserInfo.UserId &&
                c.UserInfo.UserEmail == request.UserInfo.UserEmail &&
                c.UserInfo.UserDisplayName == request.UserInfo.UserDisplayName),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_UnhandledException_Then_Returns_InternalServerError(
        Guid approvalRequestId,
        long apprenticeId,
        long accountId,
        ProcessApprenticeshipApprovalRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticeshipApprovalsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<ProcessApprenticeshipApprovalCommand>(),
                It.IsAny<CancellationToken>()))
            .Throws<ApplicationException>();

        var controllerResult = await controller.PostApprenticeshipApproval(accountId, apprenticeId, approvalRequestId, request) as StatusCodeResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}