using SFA.DAS.EmployerFinance.Api.Controllers;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;
using Microsoft.AspNetCore.Http.HttpResults;
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
}