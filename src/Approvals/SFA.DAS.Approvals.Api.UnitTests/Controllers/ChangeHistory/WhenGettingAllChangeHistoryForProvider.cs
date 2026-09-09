using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.ChangeHistory.Queries.GetAll;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.ChangeHistory;

public class WhenGettingAllChangeHistoryForProvider
{
    [Test, MoqAutoData]
    public async Task Then_Gets_All_ChangeHistory_For_Provider_From_Mediator(
       long providerId,
       GetAllChangeHistoryForProviderQueryResult mediatorResult,
       [Frozen] Mock<IMediator> mockMediator,
       [Greedy] ChangeHistoryController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetAllChangeHistoryForProviderQuery>(q => q.ProviderId == providerId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetChangeHistoryForAllLearnersOfProvider(providerId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetAllChangeHistoryForProviderQueryResult;
        model.Should().NotBeNull();
        model.ChangeHistory.Count().Should().Be(mediatorResult.ChangeHistory.Count());
        model.ChangeHistory.Should().BeEquivalentTo(mediatorResult.ChangeHistory);
    }

    [Test, MoqAutoData]
    public async Task And_No_ChangeHistory_For_Provider_Then_ReturnsEmptyResult(
         long providerId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ChangeHistoryController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetAllChangeHistoryForProviderQuery>(q => q.ProviderId == providerId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAllChangeHistoryForProviderQueryResult() { ChangeHistory = [] });

        var controllerResult = await controller.GetChangeHistoryForAllLearnersOfProvider(providerId) as ObjectResult;
        var model = controllerResult.Value as GetAllChangeHistoryForProviderQueryResult;
        model.ChangeHistory.Should().BeEmpty();
    }
}