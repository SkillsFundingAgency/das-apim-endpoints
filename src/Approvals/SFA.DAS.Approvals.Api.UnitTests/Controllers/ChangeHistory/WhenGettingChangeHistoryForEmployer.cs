using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.ChangeHistory.Queries.GetAll;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.ChangeHistory;

public class WhenGettingChangeHistoryForEmployer
{
    [Test, MoqAutoData]
    public async Task Then_Gets_ChangeHistory_For_Employer_From_Mediator(
       long accountId,
       GetAllChangeHistoryForEmployerQueryResult mediatorResult,
       [Frozen] Mock<IMediator> mockMediator,
       [Greedy] ChangeHistoryController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetAllChangeHistoryForEmployerQuery>(q => q.AccountId == accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetChangeHistoryForEmployer(accountId) as ObjectResult;
        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetAllChangeHistoryForEmployerQueryResult;
        model.Should().NotBeNull();
        model.ChangeHistory.Count().Should().Be(mediatorResult.ChangeHistory.Count());
        model.ChangeHistory.Should().BeEquivalentTo(mediatorResult.ChangeHistory);
    }

    [Test, MoqAutoData]
    public async Task And_No_ChangeHistory_For_Employer_Then_ReturnsEmptyResult(
         long accountId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ChangeHistoryController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetAllChangeHistoryForEmployerQuery>(q => q.AccountId == accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAllChangeHistoryForEmployerQueryResult() { ChangeHistory = [] });

        var controllerResult = await controller.GetChangeHistoryForEmployer(accountId) as ObjectResult;
        var model = controllerResult.Value as GetAllChangeHistoryForEmployerQueryResult;
        model.ChangeHistory.Should().BeEmpty();
    }
}