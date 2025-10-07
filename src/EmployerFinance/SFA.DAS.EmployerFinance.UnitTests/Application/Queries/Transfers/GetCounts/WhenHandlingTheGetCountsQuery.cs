using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetCounts;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.Transfers.GetCounts;

public class WhenHandlingTheGetCountsQuery
{
        [Test, MoqAutoData]
        public async Task And_AccountId_Specified_Then_Counts_Returned(
            long accountId,
            GetPledgesResponse getPledgesResponse,
            GetTransferFinancialBreakdownResponse getTransferFinancialBreakdownResponse,
            GetApplicationsResponse getApplicationsResponse,
            [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> levyTransferMatchingClient,
            [Frozen] Mock<IForecastingApiClient<ForecastingApiConfiguration>> forecastingApiConfiguration,
            GetCountsQueryHandler getCountsQueryHandler)
        {
            GetCountsQuery getCountsQuery = new GetCountsQuery()
            {
                AccountId = accountId,
            };

            levyTransferMatchingClient
                .Setup(x => x.Get<GetPledgesResponse>(It.IsAny<GetPledgesRequest>()))
                .ReturnsAsync(getPledgesResponse);

            forecastingApiConfiguration
                .Setup(x => x.Get<GetTransferFinancialBreakdownResponse>(It.IsAny<GetTransferFinancialBreakdownRequest>()))
                .ReturnsAsync(getTransferFinancialBreakdownResponse);

            levyTransferMatchingClient
                .Setup(x => x.Get<GetApplicationsResponse>(It.IsAny<GetApplicationsRequest>()))
                .ReturnsAsync(getApplicationsResponse);

            var results = await getCountsQueryHandler.Handle(getCountsQuery, CancellationToken.None);

            results.PledgesCount.Should().Be(getPledgesResponse.TotalPledges);
            results.ApplicationsCount.Should().Be(getApplicationsResponse.Applications.Count());
            results.CurrentYearEstimatedCommittedSpend.Should().Be(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.ApprovedPledgeApplications) +
                             getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.AcceptedPledgeApplications)
                             + getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.PledgeOriginatedCommitments)
                             + getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.TransferConnections));
        }
}
