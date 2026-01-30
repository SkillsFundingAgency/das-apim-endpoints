using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetSelectLegalEntity;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Cohorts;

public class WhenGettingSelectLegalEntity
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Account_From_Mediator(
        long accountId,
        GetSelectLegalEntityQueryResult mediatorResult,
        [Frozen] Mock<ISender> mockMediator,
        [Greedy] CohortController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetSelectLegalEntityQuery>(c=>c.AccountId.Equals(accountId)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.SelectLegalEntity(accountId) as ObjectResult;

        Assert.That(controllerResult, Is.Not.Null);
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetSelectLegalEntityResponse;
        Assert.That(model, Is.Not.Null);
        model.LegalEntities.Should().BeEquivalentTo(mediatorResult.LegalEntities, options=>options.Excluding(x=>x.Agreements));
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        long accountId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] CohortController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetSelectLegalEntityQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.SelectLegalEntity(accountId) as BadRequestResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}