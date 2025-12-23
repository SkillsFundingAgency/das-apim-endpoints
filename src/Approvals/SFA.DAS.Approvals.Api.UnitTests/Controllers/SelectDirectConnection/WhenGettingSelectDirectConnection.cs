using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.SelectDirectTransferConnection.Queries;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.SelectDirectConnection;

public class WhenGettingSelectDirectConnection
{
    [Test, MoqAutoData]
    public async Task Then_Get_Returns_IsLevyStatus_From_Mediator(
                long accountId,
                GetSelectDirectTransferConnectionQueryResult mediatorResult,
                [Frozen] Mock<IMediator> mockMediator,
                [Greedy] SelectDirectConnectionController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(
                It.Is<GetSelectDirectTransferConnectionQuery>(x => x.AccountId == accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.Get(accountId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetSelectDirectConnectionResponse;
        model.Should().NotBeNull();
        model.IsLevyAccount.Should().Be(mediatorResult.IsLevyAccount);
    }

    [Test, MoqAutoData]
    public async Task Then_Get_Returns_DirectConnections_From_Mediator(
        long accountId,
        GetSelectDirectTransferConnectionQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SelectDirectConnectionController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(
                It.Is<GetSelectDirectTransferConnectionQuery>(x => x.AccountId == accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.Get(accountId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetSelectDirectConnectionResponse;
        model.Should().NotBeNull();
        model.TransferConnections.Should().BeEquivalentTo(mediatorResult.TransferConnections.Select(x =>
            new GetSelectDirectConnectionResponse.TransferDirectConnection
            {
                FundingEmployerAccountId = x.FundingEmployerAccountId,
                FundingEmployerHashedAccountId = x.FundingEmployerHashedAccountId,
                FundingEmployerPublicHashedAccountId = x.FundingEmployerPublicHashedAccountId,
                FundingEmployerAccountName = x.FundingEmployerAccountName,
                ApprovedOn = x.StatusAssignedOn
            }));
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        long accountId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SelectDirectConnectionController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetSelectDirectTransferConnectionQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.Get(accountId) as StatusCodeResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}