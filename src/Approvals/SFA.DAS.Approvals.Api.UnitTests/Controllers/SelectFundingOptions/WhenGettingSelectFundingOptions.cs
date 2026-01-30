using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.SelectFunding.Queries;
using SFA.DAS.Approvals.Application.SelectProvider.Queries;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.SelectFundingOptions;

public class WhenGettingSelectFundingOptions
{
    [Test, MoqAutoData]
    public async Task Then_Get_Returns_FundingOptionsForAccount_From_Mediator(
                long accountId,
                GetSelectFundingOptionsQueryResult mediatorResult,
                [Frozen] Mock<IMediator> mockMediator,
                [Greedy] SelectFundingOptionsController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(
                It.Is<GetSelectFundingOptionsQuery>(x => x.AccountId == accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.Get(accountId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetSelectFundingOptionsQueryResult;
        Assert.That(model, Is.Not.Null);
        model.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        long accountId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SelectFundingOptionsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetSelectFundingOptionsQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.Get(accountId) as StatusCodeResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}