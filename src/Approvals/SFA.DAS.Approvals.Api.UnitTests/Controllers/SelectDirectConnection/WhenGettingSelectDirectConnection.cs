using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.SelectDirectTransferConnection.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.SelectDirectConnection;

public class WhenGettingSelectDirectConnection
{
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
        var model = controllerResult.Value as GetSelectDirectTransferConnectionQueryResult;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(mediatorResult);
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