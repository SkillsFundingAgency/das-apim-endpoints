using SFA.DAS.EmployerFinance.Api.Controllers;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsStatusByAccountId;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Net;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Controllers.FinanceLevy;

[TestFixture]
internal class WhenGettingProjectionsStatus
{
    [Test, MoqAutoData]
    public async Task Then_Gets_The_Status_From_Mediator(
        long accountId,
        GetLevyProjectionsStatusByAccountIdQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] FinanceLevyController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetLevyProjectionsStatusByAccountIdQuery>(c => c.AccountId.Equals(accountId)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetLevyProjectionsStatus(accountId);

        var okResult = controllerResult.Should().BeOfType<Ok<GetLevyProjectionsStatusByAccountIdQueryResult>>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().Be(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task Then_An_Error_Is_Returned_When_The_Status_Cannot_Be_Retrieved(
        long accountId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] FinanceLevyController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(It.IsAny<GetLevyProjectionsStatusByAccountIdQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        var controllerResult = await controller.GetLevyProjectionsStatus(accountId);

        controllerResult.Should().BeOfType<InternalServerError>();
    }
}
