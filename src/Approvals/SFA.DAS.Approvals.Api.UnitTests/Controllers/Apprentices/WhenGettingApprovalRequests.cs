using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprovalRequests;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices;

public class WhenGettingApprovalRequests
{
    [Test, MoqAutoData]
    public async Task Then_Gets_ApprovalRequest_From_Mediator(
        long apprenticeshipId,
        GetApprovalRequestQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticesController controller,
        long accountId)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetApprovalRequestQuery>(q => q.ApprenticeshipId == apprenticeshipId && q.AccountId == accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetApprovalRequest(accountId, apprenticeshipId, 1) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetApprovalRequestQueryResult;
        model.Should().NotBeNull();
        model.ApprenticeName.Should().Be(mediatorResult.ApprenticeName);
        model.ApprovalRequests.Should().BeEquivalentTo(mediatorResult.ApprovalRequests);
    }

    [Test, MoqAutoData]
    public async Task And_No_ApprovalRequests_Then_ReturnsEmptyResult(
         long apprenticeshipId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticesController controller,
        long accountId)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetApprovalRequestQuery>(q => q.ApprenticeshipId == apprenticeshipId && q.AccountId == accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetApprovalRequestQueryResult() { ApprovalRequests = [] });

        var controllerResult = await controller.GetApprovalRequest(accountId, apprenticeshipId, 1) as ObjectResult;
        var model = controllerResult.Value as GetApprovalRequestQueryResult;
        model.ApprovalRequests.Should().BeEmpty();
    }
}