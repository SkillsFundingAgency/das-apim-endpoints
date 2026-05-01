using SFA.DAS.EmployerFinance.Api.Controllers;
using SFA.DAS.EmployerFinance.Api.Models.Transfers;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetCounts;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Controllers.Transfers;

public class WhenGettingCounts
{
        [Test, MoqAutoData]
        public async Task Then_Gets_Counts_From_Mediator(
            long accountId,
            GetCountsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TransfersController transfersController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetCountsQuery>(y => y.AccountId == accountId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actionResult = await transfersController.GetCounts(accountId);
            var okObjectResult = actionResult as OkObjectResult;
            var value = okObjectResult.Value;
            var getIndexResponse = value as GetCountsResponse;

            actionResult.Should().NotBeNull();
            okObjectResult.Should().NotBeNull();
            value.Should().NotBeNull();
            getIndexResponse.Should().NotBeNull();

            getIndexResponse.PledgesCount.Should().Be(mediatorResult.PledgesCount);
            getIndexResponse.ApplicationsCount.Should().Be(mediatorResult.ApplicationsCount);
            getIndexResponse.CurrentYearEstimatedCommittedSpend.Should().Be(mediatorResult.CurrentYearEstimatedCommittedSpend);
        }
}
