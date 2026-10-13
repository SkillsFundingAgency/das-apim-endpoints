using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Commands.AcknowledgeApprovalRequestAlerts;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices;

[TestFixture]
public class WhenUpdatingApprovalRequestAlerts
{
    [Test, MoqAutoData]
    public void UpdateApprovalRequestAlerts(
        long apprenticeshipId,
        long accountId,
        UpdateApprovalRequestAlertAcknowledgeRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticesController controller)
    {
        var response = new ApiResponse<object>(null, System.Net.HttpStatusCode.OK, string.Empty);

        mockMediator
           .Setup(mediator => mediator.Send(
               It.Is<UpdateApprovalRequestAlertAcknowledgeCommand>(q => q.ApprenticeshipId == apprenticeshipId
               && q.AccountId == accountId
               && q.ApprovalRequestAlerts == request.ApprovalRequestAlerts)));

        var controllerResult = controller.UpdateApprovalRequestAlertAcknowledge(accountId, apprenticeshipId, request);

        controllerResult.Result.Should().BeOfType<OkResult>();
    }
}