using SFA.DAS.EmployerFinance.Api.Controllers;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByAccountId;
using System;
using System.Net;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Controllers.FinanceLevy;

[TestFixture]
internal class WhenGettingProjections
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Levy_Projections_From_Mediator(
        long accountId,
        int months,
        GetLevyProjectionsByAccountIdQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] FinanceLevyController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetLevyProjectionsByAccountIdQuery>(c => c.AccountId.Equals(accountId) && c.Months.Equals(months)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetLevyProjections(accountId, months) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        controllerResult.Value.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_BadRequest_When_Mediator_Throws(
        long accountId,
        int months, 
        Exception exception,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] FinanceLevyController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetLevySummaryByAccountIdQuery>(c => c.AccountId.Equals(accountId)),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        var controllerResult = await controller.GetLevySummary(accountId) as StatusCodeResult;

        controllerResult.Should().NotBeNull();
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}