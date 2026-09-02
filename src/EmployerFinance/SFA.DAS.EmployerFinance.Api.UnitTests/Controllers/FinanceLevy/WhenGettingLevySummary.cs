using SFA.DAS.EmployerFinance.Api.Controllers;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByHashedAccountId;
using System;
using System.Net;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Controllers.FinanceLevy;

[TestFixture]
internal class WhenGettingLevySummary
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Levy_Summary_From_Mediator(
        string hashedAccountId,
        GetLevySummaryByHashedAccountIdQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] FinanceLevyController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetLevySummaryByHashedAccountIdQuery>(c => c.HashedAccountId.Equals(hashedAccountId)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetLevySummary(hashedAccountId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        controllerResult.Value.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_BadRequest_When_Mediator_Throws(
        string hashedAccountId,
        Exception exception,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] FinanceLevyController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetLevySummaryByHashedAccountIdQuery>(c => c.HashedAccountId.Equals(hashedAccountId)),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        var controllerResult = await controller.GetLevySummary(hashedAccountId) as StatusCodeResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}
