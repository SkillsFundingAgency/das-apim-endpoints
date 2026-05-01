using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.AgreementNotSigned.Queries;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.AgreementNotSigned;

public class WhenGettingAgreementNotSigned
{
    [Test, MoqAutoData]
    public async Task Then_Returns_AccountStatus_From_Mediator(
                long accountId,
                GetAgreementNotSignedResult mediatorResult,
                [Frozen] Mock<IMediator> mockMediator,
                [Greedy] AgreementNotSignedController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(
                It.Is<GetAgreementNotSignedQuery>(x => x.AccountId == accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.Get(accountId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetAgreementNotSignedResult;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_NotFound_From_Mediator(
        long accountId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] AgreementNotSignedController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(
                It.IsAny<GetAgreementNotSignedQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetAgreementNotSignedResult)null);

        var controllerResult = await controller.Get(accountId) as NotFoundResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        long accountId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] AgreementNotSignedController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetAgreementNotSignedQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.Get(accountId) as StatusCodeResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}