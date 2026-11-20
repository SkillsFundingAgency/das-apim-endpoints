using SFA.DAS.EmployerFinance.Api.Controllers;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionMonthsByAccountId;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;
using Microsoft.AspNetCore.Http.HttpResults;
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

        var controllerResult = await controller.GetLevyProjections(accountId, months);

        controllerResult.Should().BeOfType<Ok<GetLevyProjectionsByAccountIdQueryResult>>();
        var okResult = (Ok<GetLevyProjectionsByAccountIdQueryResult>)controllerResult;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Download_Projects_The_Number_Of_Months_Needed_For_The_Account(
        long accountId,
        int months,
        GetLevyProjectionsByAccountIdQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] FinanceLevyController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetLevyProjectionMonthsByAccountIdQuery>(c => c.AccountId.Equals(accountId)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetLevyProjectionMonthsByAccountIdQueryResult(months));
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetLevyProjectionsByAccountIdQuery>(c => c.AccountId.Equals(accountId) && c.Months.Equals(months)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetLevyProjectionsForDownload(accountId);

        controllerResult.Should().BeOfType<Ok<GetLevyProjectionsByAccountIdQueryResult>>();
        ((Ok<GetLevyProjectionsByAccountIdQueryResult>)controllerResult).Value.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Download_Returns_An_Error_When_The_Projection_Fails(
        long accountId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] FinanceLevyController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(It.IsAny<GetLevyProjectionMonthsByAccountIdQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        var controllerResult = await controller.GetLevyProjectionsForDownload(accountId);

        controllerResult.Should().BeOfType<InternalServerError>();
    }
}
