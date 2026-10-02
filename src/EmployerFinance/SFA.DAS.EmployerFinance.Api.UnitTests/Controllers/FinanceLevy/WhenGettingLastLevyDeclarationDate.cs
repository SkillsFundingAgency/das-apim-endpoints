using SFA.DAS.EmployerFinance.Api.Controllers;
using SFA.DAS.EmployerFinance.Application.Queries.GetLastLevyDeclarationDate;
using System;
using System.Net;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Controllers.FinanceLevy;

[TestFixture]
internal class WhenGettingLastLevyDeclarationDate
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Levy_Projections_From_Mediator(
        long accountId,
        GetLastLevyDeclarationDateQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] FinanceLevyController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetLastLevyDeclarationDateQuery>(c => c.AccountId.Equals(accountId)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetLastLevyDeclarationDate(accountId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        controllerResult.Value.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_BadRequest_When_Mediator_Throws(
        long accountId,
        Exception exception,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] FinanceLevyController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetLastLevyDeclarationDateQuery>(c => c.AccountId.Equals(accountId)),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        var controllerResult = await controller.GetLastLevyDeclarationDate(accountId) as StatusCodeResult;

        controllerResult.Should().NotBeNull();
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}
