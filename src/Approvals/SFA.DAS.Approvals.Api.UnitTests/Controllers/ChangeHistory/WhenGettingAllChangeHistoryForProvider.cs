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

    [Test, MoqAutoData]
    public async Task And_Paging_Values_Are_Passed_To_Mediator(
         long providerId,
         GetAllChangeHistoryForProviderQueryResult mediatorResult,
         [Frozen] Mock<IMediator> mockMediator,
         [Greedy] ChangeHistoryController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetAllChangeHistoryForProviderQuery>(q => q.ProviderId == providerId && q.PageNumber == 3 && q.PageSize == 25),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetChangeHistoryForAllLearnersOfProvider(providerId, 3, 25) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        mockMediator.Verify(mediator => mediator.Send(
            It.Is<GetAllChangeHistoryForProviderQuery>(q => q.PageNumber == 3 && q.PageSize == 25),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Page_Size_Is_At_Maximum_Then_Request_Is_Accepted(
         long providerId,
         GetAllChangeHistoryForProviderQueryResult mediatorResult,
         [Frozen] Mock<IMediator> mockMediator,
         [Greedy] ChangeHistoryController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(It.IsAny<GetAllChangeHistoryForProviderQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetChangeHistoryForAllLearnersOfProvider(providerId, 1, 100) as ObjectResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Test, MoqAutoData]
    public async Task And_Page_Number_Is_Less_Than_One_Then_Returns_BadRequest(
         long providerId,
         [Frozen] Mock<IMediator> mockMediator,
         [Greedy] ChangeHistoryController controller)
    {
        var controllerResult = await controller.GetChangeHistoryForAllLearnersOfProvider(providerId, 0, 50);

        controllerResult.Should().BeOfType<BadRequestResult>();
        mockMediator.Verify(mediator => mediator.Send(It.IsAny<GetAllChangeHistoryForProviderQuery>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task And_Page_Size_Is_Less_Than_One_Then_Returns_BadRequest(
         long providerId,
         [Frozen] Mock<IMediator> mockMediator,
         [Greedy] ChangeHistoryController controller)
    {
        var controllerResult = await controller.GetChangeHistoryForAllLearnersOfProvider(providerId, 1, 0);

        controllerResult.Should().BeOfType<BadRequestResult>();
        mockMediator.Verify(mediator => mediator.Send(It.IsAny<GetAllChangeHistoryForProviderQuery>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task And_Page_Size_Exceeds_Maximum_Then_Returns_BadRequest(
         long providerId,
         [Frozen] Mock<IMediator> mockMediator,
         [Greedy] ChangeHistoryController controller)
    {
        var controllerResult = await controller.GetChangeHistoryForAllLearnersOfProvider(providerId, 1, 101);

        controllerResult.Should().BeOfType<BadRequestResult>();
        mockMediator.Verify(mediator => mediator.Send(It.IsAny<GetAllChangeHistoryForProviderQuery>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}