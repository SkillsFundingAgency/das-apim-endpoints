using SFA.DAS.EmployerFinance.Api.Controllers;
using SFA.DAS.EmployerFinance.Api.Models.Transfers;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetFinancialBreakdown;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Controllers.Transfers;

public class WhenGettingFinancialBreakdown
{
        [Test, MoqAutoData]
        public async Task Then_Gets_Financial_Breakdown_From_Mediator(
           long accountId,
           GetFinancialBreakdownResult mediatorResult,
           [Frozen] Mock<IMediator> mockMediator,
           [Greedy] TransfersController transfersController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetFinancialBreakdownQuery>(y => y.AccountId == accountId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actionResult = await transfersController.GetFinancialBreakdown(accountId);
            var okObjectResult = actionResult as OkObjectResult;
            var value = okObjectResult.Value;
            var getFinancialBreakdownResponse = value as GetFinancialBreakdownResponse;

            actionResult.Should().NotBeNull();
            okObjectResult.Should().NotBeNull();
            value.Should().NotBeNull();
            getFinancialBreakdownResponse.Should().NotBeNull();

            getFinancialBreakdownResponse.Commitments.Should().Be(mediatorResult.Commitments);
            getFinancialBreakdownResponse.AcceptedPledgeApplications.Should().Be(mediatorResult.AcceptedPledgeApplications);
            getFinancialBreakdownResponse.ApprovedPledgeApplications.Should().Be(mediatorResult.ApprovedPledgeApplications);
            getFinancialBreakdownResponse.TransferConnections.Should().Be(mediatorResult.TransferConnections);
        }
}
