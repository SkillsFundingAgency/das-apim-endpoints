using System.Linq;
using SFA.DAS.EmployerFinance.Api.Controllers;
using SFA.DAS.EmployerFinance.Api.Models;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetPledges;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Controllers.Pledges;

public class WhenGettingPledges
{
        [Test, MoqAutoData]
        public async Task Then_Gets_Pledges_From_Mediator(
            long accountId,
            GetPledgesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgesController pledgesController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetPledgesQuery>(y => y.AccountId == accountId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actionResult = await pledgesController.GetPledges(accountId);
            var okObjectResult = actionResult as OkObjectResult;
            var value = okObjectResult.Value;
            var getPledgesResponse = value as GetPledgesResponse;

            actionResult.Should().NotBeNull();
            okObjectResult.Should().NotBeNull();
            value.Should().NotBeNull();
            getPledgesResponse.Should().NotBeNull();

            getPledgesResponse.TotalPledges.Should().Be(mediatorResult.TotalPledges);
            mediatorResult.Pledges.Count().Should().Be(getPledgesResponse.Pledges.Count());
        }
}