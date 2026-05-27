using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.ChangeHistory.Queries;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.ChangeHistory;

public class WhenGettingChangeHistory
{
    [Test, MoqAutoData]
    public async Task Then_Gets_ChangeHistory_From_Mediator(
        long apprenticeshipId,
        GetChangeHistoryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ChangeHistoryController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetChangeHistoryQuery>(q => q.ApprenticeshipId == apprenticeshipId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetChangeHistory(apprenticeshipId) as ObjectResult;

        Assert.That(controllerResult, Is.Not.Null);
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetChangeHistoryResult;
        Assert.That(model, Is.Not.Null);
        model.ChangeHistory.Should().BeEquivalentTo(mediatorResult.ChangeHistory);
    }

    [Test, MoqAutoData]
    public async Task And_No_ChangeHistory_Then_Returns_NotFound(
         long apprenticeshipId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ChangeHistoryController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetChangeHistoryQuery>(q => q.ApprenticeshipId == apprenticeshipId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetChangeHistoryResult)null);

        var controllerResult = await controller.GetChangeHistory(apprenticeshipId) as NotFoundResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}