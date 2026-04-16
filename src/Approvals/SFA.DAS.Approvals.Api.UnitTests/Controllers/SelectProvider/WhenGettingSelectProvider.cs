using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.SelectProvider.Queries;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.SelectProvider;

public class WhenGettingSelectProvider
{
    [Test, MoqAutoData]
    public async Task Then_Get_Returns_ProviderAndAccountLegalEntity_From_Mediator(
                long accountLegalEntityId,
                GetSelectProviderQueryResult mediatorResult,
                [Frozen] Mock<IMediator> mockMediator,
                [Greedy] SelectProviderController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(
                It.Is<GetSelectProviderQuery>(x => x.AccountLegalEntityId == accountLegalEntityId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.Get(accountLegalEntityId) as ObjectResult;

        Assert.That(controllerResult, Is.Not.Null);
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetSelectProviderQueryResult;
        Assert.That(model, Is.Not.Null);
        model.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        long accountLegalEntityId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SelectProviderController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetSelectProviderQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.Get(accountLegalEntityId) as StatusCodeResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}