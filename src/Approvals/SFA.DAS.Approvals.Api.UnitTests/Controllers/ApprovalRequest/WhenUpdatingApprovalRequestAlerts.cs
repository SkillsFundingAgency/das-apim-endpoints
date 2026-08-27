using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.ApprovalRequest.Commands;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.ApprovalRequest;

[TestFixture]
public class WhenUpdatingApprovalRequestAlerts
{
    [Test, MoqAutoData]
    public void UpdateApprovalRequestAlerts(
        long apprenticeshipId,
        UpdateApprovalRequestAlertAcknowledgeRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprovalRequestController controller)
    {
        var response = new ApiResponse<object>(null, System.Net.HttpStatusCode.OK, string.Empty);

        mockMediator
           .Setup(mediator => mediator.Send(
               It.Is<UpdateApprovalRequestAlertAcknowledgeCommand>(q => q.ApprenticeshipId == apprenticeshipId
               && q.ApprovalRequestAlerts == request.ApprovalRequestAlerts)));

        var controllerResult = controller.UpdateApprovalRequestAlertAcknowledge(apprenticeshipId, request);

        controllerResult.Result.Should().BeOfType<OkResult>();
    }
}