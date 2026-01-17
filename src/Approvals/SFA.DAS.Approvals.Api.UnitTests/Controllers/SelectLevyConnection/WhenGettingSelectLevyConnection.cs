using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.LevyTransferMatching.Queries.GetApprovedAccountApplication;
using SFA.DAS.Approvals.Application.SelectDirectTransferConnection.Queries;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.SelectLevyConnection;

public class WhenGettingSelectLevyConnection
{
    [Test, MoqAutoData]
    public async Task Then_Get_Returns_LevyConnections_From_Mediator(
                long accountId,
                GetAcceptedEmployerAccountApplicationsQueryResult mediatorResult,
                [Frozen] Mock<IMediator> mockMediator,
                [Greedy] SelectAcceptedLevyApplicationsController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(
                It.Is<GetAcceptedEmployerAccountApplicationsQuery>(x => x.EmployerAccountId == accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.Get(accountId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetAcceptedEmployerAccountApplicationsQueryResult;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        long accountId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SelectAcceptedLevyApplicationsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetAcceptedEmployerAccountApplicationsQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.Get(accountId) as StatusCodeResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}