using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetCounts;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.Transfers.GetCounts
{
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

            Assert.AreEqual(getPledgesResponse.TotalPledges, results.PledgesCount);
            Assert.AreEqual(getApplicationsResponse.Applications.Count(), results.ApplicationsCount);
            Assert.AreEqual((getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.ApprovedPledgeApplications) +
                             getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.AcceptedPledgeApplications)
                             + getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.PledgeOriginatedCommitments)
                             + getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.TransferConnections)), results.CurrentYearEstimatedCommittedSpend);
        }
    }
}
