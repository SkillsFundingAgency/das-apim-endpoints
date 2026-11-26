using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetPledges;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetPledges;

public class WhenHandlingTheGetPledgesQuery
{
        [Test, MoqAutoData]
        public async Task And_AccountId_Specified_Then_Pledges_Returned(
            long accountId,
            GetPledgesResponse getPledgesResponse,
            [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> mockLevyTransferMatchingClient,
            GetPledgesQueryHandler getPledgesQueryHandler)
        {
            GetPledgesQuery getPledgesQuery = new GetPledgesQuery()
            {
                AccountId = accountId,
            };

            mockLevyTransferMatchingClient
                .Setup(x => x.Get<GetPledgesResponse>(It.IsAny<GetPledgesRequest>()))
                .ReturnsAsync(getPledgesResponse);

            var results = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            results.Pledges.Should().BeEquivalentTo(getPledgesResponse.Pledges);
            results.TotalPledges.Should().Be(getPledgesResponse.TotalPledges);
        }
}